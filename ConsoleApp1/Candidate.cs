using System;
using System.IO;
using System.Runtime.CompilerServices;
namespace ConsoleApp1
{
    public class Candidate
    {
        private int candidateID;
        private string name;
        private string party; // must be unique 
        private int votes;
        private Random rnd = new Random();
        private int GenerateCandidateID()
        {
            int R_number=rnd.Next(1,100);
            return R_number;
        }
        public Candidate(string name, string party)
        {
            candidateID = GenerateCandidateID();
            this.name = name;
            this.party = party;
            votes = 0;
        }
        public Candidate() { }
        public int CandidateID
        {
            set { candidateID = value; }
            get { return candidateID; }
        }
        public string Name
        {
            set { name = value; }
            get { return name; }
        }
        public string Party
        {
            set { party = value; }
            get { return party; }
        }
        public int Votes
        {
            set { votes = value; }
            get { return votes; }
        }
        public void IncrementVotes()
        {
            votes++;
        }

        //overriding the function ToString 
        public override string ToString()
        {
            return $" Candidate ID : {candidateID}, Name : {name} " +
                $", Party: {party}, Votes : {votes}";
        }
    }
}
