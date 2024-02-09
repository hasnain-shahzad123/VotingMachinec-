using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Voter
    {
        private string voterName;
        private string cnic;
        private string selectedPartyName;

        public string SelectedPartyName
        {
            set { selectedPartyName = value; }
            get { return selectedPartyName; }
        }
        public string VoterName
        {
            set { voterName = value; }
            get { return voterName; }
        }
        public string Cnic
        {
            set { cnic = value; }
            get { return cnic; }
        }
        //constructor 
        public Voter() { }
        public Voter(string cnic, string voterName,string selectedPartyName)
        {
            this.voterName = voterName;
            this.cnic = cnic;
            this.selectedPartyName = selectedPartyName;
        }
        //public string SelectedPartyName
        //{
        //    get { return selectedPartyName; }
        //}
        public bool hasVoted(string cnic)
        {

            FileStream fout = new FileStream("VotersDetail.txt", FileMode.Open);  
            StreamReader sr = new StreamReader(fout);
            string line;
            while((line=sr.ReadLine())!=null) 
            {
                string[] field = line.Split(',');
                if(field.Length==3) {
                    if (field[0].Trim() == cnic.Trim() )
                    {
                        if (field[2] == "Nill")
                        {
                            fout.Close();
                            return false;//means not casted the vote
                        }
                        else
                        {
                            fout.Close();
                            return true; //means casted the vote
                        }
                    }
                    
                }
            }
            fout.Close();
            return false;
        }
        public override string ToString()
        {
            return $"Name : {voterName} cnic : {cnic} partyName : {selectedPartyName}";
        }

    }
}
