using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace backend.Controllers
{
     [ApiController]
    [Route("[controller]")]
    public class TestDriveController:ControllerBase
    {
     Business.TestDriveBusiness business = new Business.TestDriveBusiness();
        Utils.TestDriveConversor conversor = new Utils.TestDriveConversor();
       
        [HttpPost("Login")]
        public  ActionResult<Models.Response.TestDriveResponse.Login> VerificarLogin(Models.Request.TestDriveRequest.Login req)
        {
            try
            {
                        
                    Models.TbLogin tb=business.VerificarLogin(req);
                    Models.TbCliente cliente=business.verificarCliente(tb);
                    Models.TbFuncionario funcionario=business.verificarFucionario(tb);
                    return conversor.ParaResponseLogin( cliente,funcionario,business.VerificarPerfil(tb));
            }
            catch (System.Exception e)
            {
                
                return new NotFoundObjectResult(new Models.Response.erro(404,e.Message));
            }
        }
        [HttpGet("Cliente/Consultar/{id}")]
          public ActionResult<List<Models.Response.TestDriveResponse.ClienteAgendamento>> ClienteAgendamentos(int id)
          {
               try
               {
                    List<Models.TbAgendamento> tb=business.AgendamentosCliente(id);
                    List<Models.Response.TestDriveResponse.ClienteAgendamento> resp=tb.Select(x=>conversor.ParaResponseagenda(x)).ToList();
                    return resp;
               }
               catch (System.Exception e)
               {
                   
                    return new NotFoundObjectResult(new Models.Response.erro(404,e.Message)); 
                
               }  
              
          }
          [HttpPost("cliente/{id}")]
          public ActionResult<Models.Response.TestDriveResponse.ClienteAgendar> agendar(Models.Request.TestDriveRequest.Agendar ag,int id)
          {
               try
               {
                    Models.TbCarro car=business.Verificarcarro(ag.Carro);
                    if(car==null)
                       return  NotFound();

                    Models.TbAgendamento tb=conversor.ParaTabelaAgenda(ag,id,car);
                    business.ValidarAgendamento(tb);
                    return conversor.ParaResponseagendar(tb);
               }
               catch (System.Exception e)
               {
                   
                   return  BadRequest(new Models.Response.erro(400,e.Message));
               }

                 
          }
          [HttpPut("feedback/{id}")]
          public ActionResult<Models.Response.TestDriveResponse.ResponseFeedback> RealizarFeedback(Models.Request.TestDriveRequest.RequestFeedback req,int id)
          {
              try
              {
                   Models.TbAgendamento tb=business.ValidarFeedback(req,id);
                    return conversor.ParaResponseFeedback(tb);
                  
              }
              catch (System.Exception e)
              {
                  
                  return BadRequest(new Models.Response.erro(400,e.Message));
              }
          }
          [HttpGet("Consultar/Carro")]
          public  ActionResult<List<Models.Response.TestDriveResponse.Carro>> ListarCarros()
          {
            try
            {
               List<Models.TbCarro> carro=business.ListarCarros();
               return carro.Select(x=>conversor.ParaResponseCarro(x)).ToList();
            }
            catch (System.Exception e)
            {
                
                return new NotFoundObjectResult(new Models.Response.erro(404,e.Message));
            }
          }
          [HttpGet("funcionarios")]
          public ActionResult<List<Models.Response.TestDriveResponse.Funcionario>> ListarFuncionarios()
          {
             try
             {
                 List<Models.TbFuncionario> funcionario=business.ListarFuncionarios();
                 return funcionario.Select(x=>conversor.ParaResponseFuncionario(x)).ToList();
             }
             catch (System.Exception e)
             {
                 
                 return new  NotFoundObjectResult(new Models.Response.erro(404,e.Message));
             }
          }

    }
                
}