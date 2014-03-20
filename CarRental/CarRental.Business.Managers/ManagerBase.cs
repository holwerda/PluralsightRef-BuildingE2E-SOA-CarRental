using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using CarRental.Business.Entities;
using CarRental.Common;

using Core.Common.Contracts;
using Core.Common.Core;
using Core.Common.Exceptions;

namespace CarRental.Business.Managers
{
    public class ManagerBase
    {
        public ManagerBase()
        {
            OperationContext context = OperationContext.Current;

            if (context != null)
            {
                _LoginName = context.IncomingMessageHeaders.GetHeader<string>("String", "System");

                if (_LoginName.IndexOf(@"\") > 1) _LoginName = string.Empty;
            }

            if (ObjectBase.Container != null)
                ObjectBase.Container.SatisfyImportsOnce(this);

            if (!string.IsNullOrWhiteSpace(_LoginName))
            {
                _AuthorizationAccount = LoadAuthorizationValidationAccount(_LoginName);
            }
        }

        protected virtual Account LoadAuthorizationValidationAccount(string loginName)
        {
            return null;
        }

        protected Account _AuthorizationAccount = null;
        protected string _LoginName;
        

        protected void ValidateAuthorization(Account entity)
        {
            if (!Thread.CurrentPrincipal.IsInRole(Security.CarRentalAdminRole))
            {
                if (_LoginName != string.Empty && entity.AccountId != _AuthorizationAccount.AccountId)
                {
                    AuthorizationValidationException ex = new AuthorizationValidationException("Attempt to access a secure record");
                    throw new FaultException<AuthorizationValidationException>(ex, ex.Message);
                }
            }
        }

        protected void ValidateAuthorization(Reservation entity)
        {
            if (!Thread.CurrentPrincipal.IsInRole(Security.CarRentalAdminRole))
            {
                if (_LoginName != string.Empty && entity.AccountId != _AuthorizationAccount.AccountId)
                {
                    AuthorizationValidationException ex = new AuthorizationValidationException("Attempt to access a secure record");
                    throw new FaultException<AuthorizationValidationException>(ex, ex.Message);
                }
            }
        }

        protected void ValidateAuthorization(Rental entity)
        {
            if (!Thread.CurrentPrincipal.IsInRole(Security.CarRentalAdminRole))
            {
                if (_LoginName != string.Empty && entity.AccountId != _AuthorizationAccount.AccountId)
                {
                    AuthorizationValidationException ex = new AuthorizationValidationException("Attempt to access a secure record");
                    throw new FaultException<AuthorizationValidationException>(ex, ex.Message);
                }
            }
        }

        protected T ExecuteFaultHandledOperation<T>(Func<T> codeToExecute)
        {
            try
            {
                return codeToExecute.Invoke();
            }
            catch (FaultException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
            
        }

        protected void ExecuteFaultHandledOperation(Action codeToExecute)
        {
            try
            {
                codeToExecute.Invoke();
            }
            catch (FaultException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }

        }
    }
}
