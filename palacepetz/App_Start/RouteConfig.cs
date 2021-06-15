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

            //  Route to register address
            routes.MapRoute(
                name: "registeraddress",
                url: "registrar-endereco",
                defaults: new { controller = "Usuario", action = "AddressRegistration", id = UrlParameter.Optional }
            );

            //  Route to register address
            routes.MapRoute(
                name: "registercard",
                url: "registrar-cartao",
                defaults: new { controller = "Usuario", action = "RegisterCard", id = UrlParameter.Optional }
            );

            //  Route to user profile
            routes.MapRoute(
                name: "profile",
                url: "perfil",
                defaults: new { controller = "Usuario", action = "Profile", id = UrlParameter.Optional }
            );

            //  Route to edit user profile
            routes.MapRoute(
                name: "editprofile",
                url: "editar-perfil",
                defaults: new { controller = "Usuario", action = "EditProfile", id = UrlParameter.Optional }
            );

            //  Route to Shedule Appointment
            routes.MapRoute(
                name: "sheduleappointment",
                url: "agendar-consulta",
                defaults: new { controller = "Home", action = "Schedule_Appointment", id = UrlParameter.Optional }
            );

            //  Route to My Cards
            routes.MapRoute(
                name: "mycards",
                url: "meus-cartoes",
                defaults: new { controller = "Usuario", action = "MyCards", id = UrlParameter.Optional }
            );

            //  Route to Remove User Card
            routes.MapRoute(
                name: "removecard",
                url: "remover-cartao/{cd_card}",
                defaults: new { controller = "Usuario", action = "RemoveCard", cd_card = UrlParameter.Optional }
            );

            //  Route to See Services
            routes.MapRoute(
                name: "services",
                url: "servicos",
                defaults: new { controller = "Home", action = "Services", id_user = UrlParameter.Optional }
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
            routes.MapRoute(//  Route to see all products with same especie
                name: "productswithspecies",
                url: "produtos/filtro/especie/{filter}",
                defaults: new { controller = "Home", action = "Products", filter = UrlParameter.Optional }
            );
            routes.MapRoute(//  Route to see all products
                name: "productswithname",
                url: "produtos/filter/nome/{filter}",
                defaults: new { controller = "Home", action = "Products", filter = UrlParameter.Optional }
            );


            routes.MapRoute(//  Route to see products details
                name: "details",
                url: "detalhes/{cd_prod}",
                defaults: new { controller = "Home", action = "Details", cd_prod = UrlParameter.Optional }
            );


            routes.MapRoute(//  Route to go to the cart
                name: "shoppingcart",
                url: "meu-carrinho",
                defaults: new { controller = "Home", action = "ShoppingCart" }
            );

            routes.MapRoute(//  Route to remove item from cart
                name: "removeitemfromcart",
                url: "meu-carrinho/remover/{cd_cart}/{id_user}",
                defaults: new { controller = "Home", action = "ShoppingCart" }
            );

            routes.MapRoute(//  Route to finish purchase
                name: "finishpurchase",
                url: "finalizar-compra",
                defaults: new { controller = "Home", action = "FinishPurchase" }
            );

            routes.MapRoute(//  Route to see user orders
                name: "myorders",
                url: "meus-pedidos",
                defaults: new { controller = "Usuario", action = "UserOrders" }
            );

            routes.MapRoute(//  Route to follow Order
                name: "followorder",
                url: "pedido/{cd_order}",
                defaults: new { controller = "Usuario", action = "FollowOrder" }
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
