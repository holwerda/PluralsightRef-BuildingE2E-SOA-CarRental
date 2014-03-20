using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Transactions;

using CarRental.Business.Bootstrapper;
using CarRental.Business.Entities;
using CarRental.Business.Managers.Managers;

using Core.Common.Core;

using SM = System.ServiceModel;
using Timer = System.Timers.Timer;

namespace CarRental.ServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            GenericPrincipal principal = new GenericPrincipal(new GenericIdentity("Miguel"), new string[] { "CarRentalAdmin" });

            Thread.CurrentPrincipal = principal;

            ObjectBase.Container = MEFLoader.Init();

            Console.WriteLine("Starting up services");
            Console.WriteLine("");

            SM.ServiceHost hostInventoryManager = new SM.ServiceHost(typeof(InventoryManager));
            SM.ServiceHost hostRentalManager = new SM.ServiceHost(typeof(RentalManager));
            SM.ServiceHost hostAccountManager = new SM.ServiceHost(typeof(AccountManager));

            StartService(hostInventoryManager, "InventoryManager");
            StartService(hostRentalManager, "RentalManager");
            StartService(hostAccountManager, "AccountManager");

            System.Timers.Timer timer = new Timer(10000);
            timer.Elapsed += OnTimerElapsed;
            timer.Start();


            Console.WriteLine("");
            Console.WriteLine("Press [Enter] to exit.");
            Console.ReadLine();

            timer.Stop();

            Console.WriteLine("Reservation Monitor Stopped");

            StopService(hostInventoryManager, "InventoryManager");
            StopService(hostRentalManager, "RentalManager");
            StopService(hostAccountManager, "AccountManager");

        }

        private static void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            var timer = (Timer)sender;
            timer.Stop();
            
            Console.WriteLine("Looking for dead reservations as {0}", DateTime.Now.ToString());

            RentalManager rentalManager = new RentalManager();

            Reservation[] reservations = rentalManager.GetDeadReservations();
            if (reservations != null)
            {
                foreach (var reservation in reservations)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            rentalManager.CancelReservation(reservation.ReservationId);
                            Console.WriteLine("Cancelling reservation '{0}'", reservation.ReservationId);
                            scope.Complete();
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("There was an exception when attempting to cancel reservation '{0}'", reservation.ReservationId);
                        }
                        
                    }
                }
            }

            timer.Start();
        }

        static void StartService(SM.ServiceHost host, string serviceDescription)
        {
            host.Open();
            Console.WriteLine("Service {0} started.", serviceDescription);

            foreach (var endpoint in host.Description.Endpoints)
            {
                Console.WriteLine("Listening on endpoint:");
                Console.WriteLine("Address: {0}", endpoint.Address.Uri);
                Console.WriteLine("Binding: {0}", endpoint.Binding.Name);
                Console.WriteLine("Contract: {0}", endpoint.Contract.Name);
            }

            Console.WriteLine();

        }

        static void StopService(SM.ServiceHost host, string serviceDescription)
        {
            host.Close();
            Console.WriteLine("Service {0} Stopped.", serviceDescription);
        }
    }
}
