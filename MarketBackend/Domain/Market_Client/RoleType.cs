﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Market_Client
{
    public abstract class RoleType
    {
        public string roleName { get; private set; }
        protected HashSet<Permission> permissions;

        public RoleType(string roleName)
        {
            this.roleName = roleName;
            permissions = new HashSet<Permission>();
        }

        //default is *can* do, and according to specific roles, false will be returned from override
        public virtual bool canAddProduct() { return true; }
        public virtual bool canRemoveProduct() { return true; }
        public virtual bool canOpenStore() { return true; }
        public virtual bool canCloseStore() { return true; }
        public virtual bool canUpdateProductPrice() { return true; }
        public virtual bool canUpdateProductDiscount() { return true; }
        public virtual bool canUpdateProductQuantity() { return true; }
        public virtual bool canAddStaffMember() { return true; }
        public virtual bool canRemoveStaffMember() { return true; }

        public virtual bool hasPermission(Permission permission)
        {
            return permissions.Contains(permission);
        }

        public virtual void addPermission(Permission permission)
        {
            permissions.Add(permission);
        }
        public virtual void removePermission(Permission permission)
        {
            permissions.Remove(permission);
        }

        public virtual IReadOnlyCollection<Permission> getPermissions() {
            return permissions;
        }
    }
}