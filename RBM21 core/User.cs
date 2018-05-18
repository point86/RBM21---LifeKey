﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBM21_core
{   
    class User
    {
        public int Position { get; set; }
        public string Nome { get; set; }
        public string Key { get; set; }        
        public int CreditoResiduo { get; set; }   
        public bool Active { get; set; }
        public DateTime Time { get; set; } //used by FileReader - RBM21Core
        public List<string> Entrances; //user by HardwareSync() - RBM21Core
       

        public User() { //non initialized data
            this.Position = -1;
            this.Nome = " --EMPTY-- ";
            this.Key  = " --EMPTY-- ";
            this.CreditoResiduo = -1;
            this.Entrances = new List<string>();       
            this.Active = false;       
        }
        public User(string Nome, string Key, int CreditoResiduo, int Position, List<string> Entrances, bool Active)
        {
            this.Position = Position;
            this.Nome = Nome;
            this.Key = Key;
            this.CreditoResiduo = CreditoResiduo;
            this.Entrances = Entrances;
            this.Active = Active;
        }       
        public override string ToString()
        { 
            return String.Format("Nome: {0}, Key: {1}, Credito Residuo: {2}",this.Nome, this.Key, this.CreditoResiduo);
        }
        public string ToVerticalString()
        { 
            return String.Format("Nome: {0}\r\nKey: {1}\r\nCredito Residuo: {2}",
                                        this.Nome, this.Key, this.CreditoResiduo);
        }
    }
   
}
