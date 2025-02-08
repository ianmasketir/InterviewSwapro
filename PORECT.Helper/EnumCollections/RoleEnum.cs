using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PORECT.Helper
{
    public enum RoleEnum
    {
        Admin = 1,
        Customer = 2
    }

    public static class RoleEnum_Extensions
    {
        public static RoleEnum ToRoleEnum(this string value)
        {
            switch (value.ToLower())
            {
                case "admin":
                    return RoleEnum.Admin;
                case "customer":
                    return RoleEnum.Customer;
                default:
                    throw new Exception("Role enum not available");
            }
        }
        public static string Description(this RoleEnum value)
        {
            switch (value)
            {
                case RoleEnum.Admin:
                    return "Administrator";
                case RoleEnum.Customer:
                    return "Customer";
                default:
                    return string.Empty;
            }
        }
    }

}
