using M17E_TrabalhoModelo_2020_21_WIP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace M17E_TrabalhoModelo_2020_21_WIP.Models
{
    public class AppRoleProvider : RoleProvider
    {
        private M17E_TrabalhoModelo_2020_21_WIPContext db = new M17E_TrabalhoModelo_2020_21_WIPContext();
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
        //devolve os perfis do utilizador
        public override string[] GetRolesForUser(string username)
        {
            try
            {
                var user = db.Users.Where(u => u.nome == username).First();
                if (user == null) throw new Exception("");
                if (user.perfil == 0)
                    return new string[] { "Administrador" };
                else
                    return new string[] { "Utilizador" };
            }
            catch
            {
                return new string[] { "" };
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }
        //testa se o utilizador tem um perfil
        public override bool IsUserInRole(string username, string roleName)
        {
            try
            {
                var user = db.Users.Where(u => u.nome == username).First();
                if (user.perfil != 0 && roleName == "Administrador")
                    throw new Exception("");
                if (user.perfil != 1 && roleName == "Utilizador")
                    throw new Exception("");

                return true;
            }
            catch
            {
                return false;
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
        //verifica se um perfil existe
        public override bool RoleExists(string roleName)
        {
            return roleName == "Administrador" || roleName == "Utilizador";
        }
    }
}