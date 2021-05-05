using StoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace StoreAPI.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            StoreContext db = new StoreContext();

            string[] roles = new string[] { };

            Customer customer = db.Customers.FirstOrDefault(u => u.login == username);
            if (customer != null)
            {
                Role customerRole = db.Roles.Find(customer.roleid);

                if (customerRole!= null) 
                    roles = new string[] { customer.role.name };
            }
            return roles;
            
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {

            StoreContext db = new StoreContext();

            Customer customer = db.Customers.FirstOrDefault(u => u.login == username);

            if (customer != null)
            {
                Role userRole = db.Roles.Find(customer.roleid);
                
                if (userRole != null & userRole.name == roleName)
                    return true;
            }

            return false;

        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}