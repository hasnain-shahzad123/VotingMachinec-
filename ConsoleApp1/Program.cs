using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("\t\t\t\t----------Welcome to Online Voting system----------\n");
            Console.WriteLine("1. Add Voter");
            Console.WriteLine("2. Update Voter");
            Console.WriteLine("3. Delete Voter ");
            Console.WriteLine("4. Display Voters");
            Console.WriteLine("5. Cast Vote");
            Console.WriteLine("6. insert Candidate");
            Console.WriteLine("7. Update Candidate");
            Console.WriteLine("8. Display Candidate");
            Console.WriteLine("9. Delete Candidate");
            Console.WriteLine("10.Declare Winner");
            Console.WriteLine("11.Clear screen");
            VoterMachine machine = new VoterMachine();
            Candidate c;
            machine.PreLoadVoters();
            machine.preLoadCandidates();
            Console.Write("Enter your choice from 1 to 10: ");
            string input =Console.ReadLine();
            int choice;
                if (int.TryParse(input, out choice))
                {
                while (choice!=-1) {
                    switch (choice)
                    {
                        case 1:
                            {
                                Console.WriteLine("------------------");
                                Console.WriteLine("1- Add voter ");
                                Console.WriteLine("------------------");
                                Console.Write("\nEnter your Cnic ");
                                string cnic = Console.ReadLine();
                                Console.Write("\nEnter your Name ");
                                string name = Console.ReadLine();
                                machine.addVoter(cnic, name);
                                break;
                            }
                        case 2:
                            {
                                Console.WriteLine("------------------");
                                Console.WriteLine("2- Update voter ");
                                Console.WriteLine("------------------");
                                Console.Write("\nEnter your Cnic ");
                                string cnic = Console.ReadLine();
                                bool res = machine.UpdateVoter(cnic);
                                if (res)
                                {
                                    Console.WriteLine("Voter Updated");
                                }
                                else
                                {
                                    Console.WriteLine("Voter does not found");
                                }
                                break;
                            }
                        case 3:
                            {
                                Console.WriteLine("------------------");
                                Console.WriteLine("3- Delete voter ");
                                Console.WriteLine("------------------");
                                Console.Write("\nEnter the Cnic ");
                                string cnic = Console.ReadLine();
                                machine.DeleteVoter(cnic);
                                break;
                            }
                        case 4:
                            {
                                Console.WriteLine("------------------");
                                Console.WriteLine("4- Display voters ");
                                Console.WriteLine("------------------");
                                machine.displayVoters();
                                break;
                            }
                        case 5:
                            {
                                Console.WriteLine("------------------");
                                Console.WriteLine("5- Caste Vote ");
                                Console.WriteLine("------------------");
                                //CasteVote()
                                Console.Write("\nEnter the Name of Candidate: ");
                                string name = Console.ReadLine();
                                Console.Write("\nEnter the name of Party: ");
                                string party = Console.ReadLine();
                                c = new Candidate(name,party);
                                Console.WriteLine("\nEnter name of Voter: ");
                                string vName = Console.ReadLine();
                                Console.WriteLine("\nEnter Cnic of Voter: ");
                                string vCnic = Console.ReadLine();
                                Voter v = new Voter { VoterName=vName,
                                Cnic=vCnic};
                                machine.castVote(c, v);
                                break;
                            }
                        case 6:
                            {
                                Console.WriteLine("------------------");
                                Console.WriteLine("7- insert Candidate ");
                                Console.WriteLine("------------------");
                                Console.Write("\nEnter the name of Candidate: ");
                                string name = Console.ReadLine();
                                Console.Write("\nEnter the name of Party: ");
                                string party = Console.ReadLine();
                                c = new Candidate(name, party);
                                machine.insertCandidate(c);
                                break;
                            }
                        case 7:
                            {
                                Console.WriteLine("------------------");
                                Console.WriteLine("7- Update Candidate ");
                                Console.WriteLine("------------------");
                                Console.Write("\nEnter the id : ");
                                int id = Convert.ToInt32(Console.ReadLine());
                                Console.Write("\nEnter New name of Candidate: ");
                                string name = Console.ReadLine();
                                Console.Write("\nEnter New name of Party: ");
                                string party = Console.ReadLine();
                                c = new Candidate(name, party);
                                if (machine.UpdateCandidate(c, id))
                                {
                                    Console.WriteLine("Candidate Updated ");
                                }
                                else
                                {
                                    Console.WriteLine("Candidate not exist");
                                }
                                break;
                            }
                        case 8:
                            {
                                Console.WriteLine("------------------");
                                Console.WriteLine("8- Display Candidates ");
                                Console.WriteLine("------------------");
                                machine.displayCandidates();
                                break;
                            }
                        case 9:
                            {
                                Console.WriteLine("------------------");
                                Console.WriteLine("9- Delete Candidate ");
                                Console.WriteLine("------------------");
                                Console.Write("\nEnter the id : ");
                                int id = Convert.ToInt32(Console.ReadLine());
                                machine.DeleteCandidate(id);  
                                break;
                            }
                        case 10:
                            {
                                Console.WriteLine("------------------");
                                Console.WriteLine("10- Declare winner ");
                                Console.WriteLine("------------------");
                                machine.DeclareWinner();
                                break;
                            }
                        case 11:
                            {
                                Console.Clear();
                                break;
                            }
                    }
                      
                    Console.WriteLine("\t\t\t\t----------Welcome to Online Voting system----------\n");
                    Console.WriteLine("1. Add Voter");
                    Console.WriteLine("2. Update Voter");
                    Console.WriteLine("3. Delete Voter ");
                    Console.WriteLine("4. Display Voters");
                    Console.WriteLine("5. Cast Vote");
                    Console.WriteLine("6. insert Candidate");
                    Console.WriteLine("7. Update Candidate");
                    Console.WriteLine("8. Display Candidate");
                    Console.WriteLine("9. Delete Candidate");
                    Console.WriteLine("10.Declare Winner");
                    Console.WriteLine("11.Clear Screen");
                    Console.Write("\nEnter your choice from 1 to 10: ");
                    input = Console.ReadLine();
                    int.TryParse(input, out choice);
                }
                }
                else
                {
                    Console.WriteLine("Error Enter valid number ");
                }








            string dummy = Console.ReadLine();
           

        }
    }
}
