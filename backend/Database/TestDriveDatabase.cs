using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace backend.Database
{
    public class TestDriveDatabase
    { 
        Models.TestDriveContext ctx = new Models.TestDriveContext();
        public Models.TbLogin verificarLogin(Models.Request.TestDriveRequest.Login req)
        {
               Models.TbLogin tabela=ctx.TbLogin.FirstOrDefault(x=>x.DsUsername==req.UserName&&
                                                                x.DsSenha==req.Senha);
                return tabela;
        }
        public string VerificarPerfil(Models.TbLogin tb)
        {
            Models.TbCliente tabela=ctx.TbCliente.FirstOrDefault(x=>x.IdLogin==tb.IdLogin);
            if(tabela==null)
                 return "Funcionário";
                 
            else 
             return "Cliente";     
                      
        }
        
        public Models.TbCliente verificarCliente(Models.TbLogin tb)
        {
               Models.TbCliente tabela = ctx.TbCliente.FirstOrDefault(x=>x.IdLogin==tb.IdLogin);
                return tabela;
        }
            public Models.TbFuncionario verificarFuncionario(Models.TbLogin tb)
        {
               Models.TbFuncionario tabela=ctx.TbFuncionario.FirstOrDefault(x=>x.IdLogin==tb.IdLogin);
                return tabela;
        }
        public List<Models.TbAgendamento> Agendamentos (int id)
        {
            List<Models.TbAgendamento> agendamento=ctx.TbAgendamento.Include(x=>x.IdClienteNavigation)
                                                                     .Include(x=>x.IdFuncionarioNavigation)
                                                                     .Include(x=>x.IdCarroNavigation)
                                                                     .Where(x=>x.IdClienteNavigation.IdLogin==id).ToList();
            return agendamento;
        }
        public Models.TbAgendamento Agendamento(Models.TbAgendamento ag)
        {
            ctx.Add(ag);
            ctx.SaveChanges();
            return ag;
        }
        public Models.TbCarro VerificarCarro(string cr)
        {
            Models.TbCarro car = ctx.TbCarro.FirstOrDefault(x=>x.DsModelo==cr);
              

            return car;
        }
        public Models.TbAgendamento Feedback(Models.Request.TestDriveRequest.RequestFeedback req,int id)
        {
                Models.TbAgendamento fed=ctx.TbAgendamento.FirstOrDefault(x=>x.IdAgendamento==id);
                fed.VlFeedback=req.Feedback;
                ctx.SaveChanges();
                return fed;
        }
        public List<Models.TbCarro> ListarCarros()
        {
           return ctx.TbCarro.ToList();
        }
        public List<Models.TbFuncionario> ListarFuncionarios()
        {
            return ctx.TbFuncionario.ToList();
        }
   
    }
}