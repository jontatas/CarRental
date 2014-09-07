﻿using Core.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Client.Entities
{
    public class Account : ObjectBase
    {
        int _AccountId;
        string _LoginEmail;
        string _FirstName;
        string _LastName;
        string _Address;
        string _City;
        string _State;
        string _ZipCode;
        string _CreditCard;
        string _ExpiryDate;

        public int AccountId
        {
            get { return _AccountId; }
            set
            {
                if(_AccountId != value)
                {
                    _AccountId = value;
                    OnPropertyChanged(() => AccountId);
                }
            }
        }

        public string LoginEmail
        {
            get { return _LoginEmail; }
            set
            {
                if (_LoginEmail != value)
                {
                    _LoginEmail = value;
                    OnPropertyChanged(() => LoginEmail);
                }
            }
        }

        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                if (_FirstName != value)
                {
                    _FirstName = value;
                    OnPropertyChanged(() => FirstName);
                }
            }
        }

        public string LastName
        {
            get { return _LastName; }
            set
            {
                if (_LastName != value)
                {
                    _LastName = value;
                    OnPropertyChanged(() => LastName);
                }
            }
        }

        public string Address
        {
            get { return _Address; }
            set
            {
                if (_Address != value)
                {
                    _Address = value;
                    OnPropertyChanged(() => Address);
                }
            }
        }

        public string City
        {
            get { return _City; }
            set
            {
                if (_City != value)
                {
                    _City = value;
                    OnPropertyChanged(() => City);
                }
            }
        }

        public string State
        {
            get { return _State; }
            set
            {
                if (_State != value)
                {
                    _State = value;
                    OnPropertyChanged(() => _State);
                }
            }
        }

        public string ZipCode
        {
            get { return _ZipCode; }
            set
            {
                if (_ZipCode != value)
                {
                    _ZipCode = value;
                    OnPropertyChanged(() => ZipCode);
                }
            }
        }

        public string CreditCard
        {
            get { return _CreditCard; }
            set
            {
                if (_CreditCard != value)
                {
                    _CreditCard = value;
                    OnPropertyChanged(() => CreditCard);
                }
            }
        }

        public string ExpiryDate
        {
            get { return _ExpiryDate; }
            set
            {
                if (_ExpiryDate != value)
                {
                    _ExpiryDate = value;
                    OnPropertyChanged(() => ExpiryDate);
                }
            }
        }
    }
}
