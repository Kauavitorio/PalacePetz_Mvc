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

            //  Route to login
            routes.MapRoute(
                name: "login",
                url: "login",
                defaults: new { controller = "Usuario", action = "Login", id = UrlParameter.Optional }
            );

            //  Route to logout
            routes.MapRoute(
                name: "logout",
                url: "logout/{logoutid}",
                defaults: new { controller = "Usuario", action = "Login", id = UrlParameter.Optional }
            );

            //  Route to create new Account
            routes.MapRoute(
                name: "createaccount",
                url: "criarconta",
                defaults: new { controller = "Usuario", action = "CreateAccount", id = UrlParameter.Optional }
            );

            //  Route to request reset password
            routes.MapRoute(
                name: "recuperarsenha",
                url: "recuperarsenha",
                defaults: new { controller = "Usuario", action = "RecuperarSenha", id = UrlParameter.Optional }
            );

            //  Route to make sure your will not join accidentally on set new password
            routes.MapRoute(
                name: "recuperarsenhaset",
                url: "novasenha",
                defaults: new { controller = "Usuario", action = "RecuperarSenha", id = UrlParameter.Optional }
            );

            //  Route to set new password
            routes.MapRoute(
                name: "setnewpassword",
                url: "novasenha/{requestId}/{userId}",
                defaults: new { controller = "Usuario", action = "SetNewPassword", id = UrlParameter.Optional }
            );

            //  Route to register new Product
            routes.MapRoute(
                name: "registernewprod",
                url: "funcionario/registrarnovoproduto",
                defaults: new { controller = "Employee", action = "RegisterProduct", id = UrlParameter.Optional }
            );

            /***************** Products Routes *******************/
            routes.MapRoute(//  Route to see all products
                name: "products",
                url: "produtos/{filter}",
                defaults: new { controller = "Home", action = "Products", filter = UrlParameter.Optional }
            );
            routes.MapRoute(//  Route to see all products
                name: "productsprice",
                url: "produtos/filtro/{filter}",
                defaults: new { controller = "Home", action = "Products", filter = UrlParameter.Optional }
            );
            routes.MapRoute(//  Route to see all products
                name: "productswithspecies",
                url: "produtos/filtro/especie/{filter}",
                defaults: new { controller = "Home", action = "Products", filter = UrlParameter.Optional }
            );



            //  Default Route
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
