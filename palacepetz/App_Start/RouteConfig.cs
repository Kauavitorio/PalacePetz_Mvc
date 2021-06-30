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

            routes.MapRoute(//  Route to see user pets
                name: "mypets",
                url: "meus-pets",
                defaults: new { controller = "Usuario", action = "MyPets" }
            );

            routes.MapRoute(//  Route to edit pet
                name: "editpet",
                url: "perfil/pet/{cd_animal}",
                defaults: new { controller = "Usuario", action = "EditMyPets" }
            );

            routes.MapRoute(//  Route to register aniaml
                name: "registermyanimal",
                url: "perfil/meus-pets/registrar-pet",
                defaults: new { controller = "Usuario", action = "RegisterMyPet" }
            );

            routes.MapRoute(//  Route to remove aniaml
                name: "removepet",
                url: "perfil/meus-pet/remover/{cd_animal}",
                defaults: new { controller = "Usuario", action = "RemovePet" }
            );

            routes.MapRoute(//  Route to Scheduled Services
                name: "schedules",
                url: "meus-agendamentos",
                defaults: new { controller = "Home", action = "ScheduledServices" }
            );

             routes.MapRoute(//  Route to Cancel Services
                name: "cancelservices",
                url: "cancelar/servicos/{cd_schedule}",
                defaults: new { controller = "Home", action = "ScheduledCancel" }
            );

            //  Route to Shedule Bath
            routes.MapRoute(
                name: "shedulebath",
                url: "agendar-banho-tosa",
                defaults: new { controller = "Home", action = "Schedule_Bath_and_tosa" }
            );

            routes.MapRoute(//  Route to Help Center
                name: "helpcenter",
                url: "suporte",
                defaults: new { controller = "Home", action = "HelpCenter" }
            );

            /* ---------- Employee Routs ---------- */


            
            routes.MapRoute( // Route to List Users
                name: "funclistusers",
                url: "clientes",
                defaults: new { controller = "Employee", action = "CheckCustommers" }
            );

            routes.MapRoute(//  Route to edit user
                name: "edituser",
                url: "funcionario/editar-cliente/{id_user_edit}",
                defaults: new { controller = "Employee", action = "EditCustommers" }
            );

            routes.MapRoute(//  Route to disabled user
                name: "disableduser",
                url: "funcionario/desativar/cliente/{id_user_disable}",
                defaults: new { controller = "Employee", action = "DisabledUser" }
            );

            routes.MapRoute(//  Route to enable user
                name: "enableuser",
                url: "funcionario/ativar/cliente/{id_user_enable}",
                defaults: new { controller = "Employee", action = "EnableUser" }
            );

            routes.MapRoute( // Route to List User Services
                name: "userservices",
                url: "funcionario/servicos",
                defaults: new { controller = "Employee", action = "UserScheduledServices" }
            );

            routes.MapRoute( //  Route to See Details Services
                name: "detailsservices",
                url: "funcionario/servicos/detalhes/{cd_schedule}/{id_user}",
                defaults: new { controller = "Employee", action = "DatailsScheduledServices" }
                
            );


            routes.MapRoute(//  Route to Statistics
                name: "statistics",
                url: "empresa/estatisticas",
                defaults: new { controller = "Employee", action = "Statistics" }
            );

            routes.MapRoute(//  Route to employee index
                name: "homeemployee",
                url: "funcionario",
                defaults: new { controller = "Employee", action = "Index"}
            );

            routes.MapRoute(//  Route to See Functions Products
                name: "seefunctions",
                url: "funcionario/funcoes/produtos",
                defaults: new { controller = "Employee", action = "FunctionsProducts" }
            );

            routes.MapRoute(//  Route to See Products
                name: "seeproductsfunc",
                url: "funcionario/produtos/{filter}",
                defaults: new { controller = "Employee", action = "ListProducts", filter = UrlParameter.Optional }
            );

            routes.MapRoute(//  Route to register new Product
                name: "registernewprod",
                url: "funcionario/registrar/produto",
                defaults: new { controller = "Employee", action = "RegisterProduct"}
            );

            routes.MapRoute(//  Route to edit Product
                name: "editprod",
                url: "funcionario/editar/{cd_prod}",
                defaults: new { controller = "Employee", action = "EditProduct" }
            );

            routes.MapRoute(//  Route to delete Product
                name: "removeprod",
                url: "funcionario/remove/{cd_prod}",
                defaults: new { controller = "Employee", action = "DeleteProduct"}
            );

            routes.MapRoute(//  Route to edit Users
                name: "edituserfunc",
                url: "funcionario/clientes/{cd_prod}",
                defaults: new { controller = "Employee", action = "Edit" }
            );

            routes.MapRoute(//  Route to see supplier
                name: "seesuplier",
                url: "funcionario/fornecedores",
                defaults: new { controller = "Manager", action = "SeeSupplier" }
            );

            routes.MapRoute( // Route to information
                name: "funcinformations",
                url: "funcionario/informacoes",
                defaults: new { controller = "Employee", action = "Informations" }
            );

            routes.MapRoute( //  Route to AllOrder
                name: "allorders",
                url: "funcionario/pedidos",
                defaults: new { controller = "Employee", action = "AllOrder" }
            );

            routes.MapRoute( //  Route to Order Control
                name: "controlorders",
                url: "funcionario/pedidos/{cd_order}",
                defaults: new { controller = "Employee", action = "OrderControl" }
            );


            /* ---------- Manager Routes ---------- */
            routes.MapRoute(//  Route to register new employee
                name: "registeremployee",
                url: "gerente/registrar-funcionario",
                defaults: new { controller = "Manager", action = "RegisterEmployee" }
            );

            routes.MapRoute(//  Route to list employee
                name: "listemployee",
                url: "gerente/funcionarios",
                defaults: new { controller = "Manager", action = "EmployeeList" }
            );

            routes.MapRoute(//  Route to edit employee
                name: "editemployee",
                url: "gerente/editar-funcionario/{id_employee}",
                defaults: new { controller = "Manager", action = "EditEmployee" }
            );

            routes.MapRoute(//  Route to delete employee
                name: "deleteemployee",
                url: "gerente/funcionario/excluir/{id_employee}",
                defaults: new { controller = "Manager", action = "RemoveEmployee" }
            );

            routes.MapRoute(//  Route to order for supplier
               name: "order-for-supplier",
               url: "gerente/pedido-fornecedor",
               defaults: new { controller = "Manager", action = "Order_for_Supplier" }
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
