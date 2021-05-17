using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace palacepetz
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "login",
                url: "login",
                defaults: new { controller = "Usuario", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "createaccount",
                url: "criarconta",
                defaults: new { controller = "Usuario", action = "CreateAccount", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "recuperarsenha",
                url: "recuperarsenha",
                defaults: new { controller = "Usuario", action = "RecuperarSenha", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "recuperarsenhaset",
                url: "novasenha",
                defaults: new { controller = "Usuario", action = "RecuperarSenha", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "setnewpassword",
                url: "novasenha/{requestId}/{userId}",
                defaults: new { controller = "Usuario", action = "SetNewPassword", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "registernewprod",
                url: "funcionario/registrarnovoproduto",
                defaults: new { controller = "Employee", action = "RegisterProduct", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
