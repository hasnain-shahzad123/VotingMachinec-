using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.IO.Ports;
namespace ConsoleApp1
{
    public class VoterMachine
    {
       private List<Candidate> candidates = new List<Candidate>();
       private List<Voter> voterList = new List<Voter>();
        public VoterMachine() { }
        public void PreLoadVoters()
        {
           
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=infoManager;Integrated Security=True";
            SqlConnection conection = new SqlConnection(connectionString);
            conection.Open();
            string query = "Select * from Voter";
            SqlCommand cmd = new SqlCommand(query,conection);
            SqlDataReader dr = cmd.ExecuteReader();
            Voter newVoter;
            while (dr.Read())
            {
                string v = Convert.ToString(dr[0]);
                string vN = Convert.ToString(dr[1]);
                string vP = Convert.ToString(dr[2]);
                newVoter = new Voter { VoterName = vN, Cnic = v, SelectedPartyName = vP };
                voterList.Add(newVoter);
            }
            conection.Close();
        }
        public void preLoadCandidates()
        { 

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=infoManager;Integrated Security=True";
            SqlConnection conection = new SqlConnection(connectionString);
            conection.Open();
            string query = "Select * from Candidate";
            SqlCommand cmd = new SqlCommand(query, conection);
            SqlDataReader dr = cmd.ExecuteReader();
            Candidate newCandidate;
            while (dr.Read())
            {
                int cId = Convert.ToInt32(dr[0]);
                string cN = Convert.ToString(dr[1]);
                string cP = Convert.ToString(dr[2]);
                int cV = Convert.ToInt32(dr[3]);
                newCandidate = new Candidate { CandidateID=cId,Name=cN,Party=cP,Votes=cV};
                candidates.Add(newCandidate);
            }
            conection.Close();

        }
        private bool isCandidateExist(string N,string PN)
        {
            //cheking if the candidate exist 
            
            foreach (Candidate can in candidates)
            {
                if (can.Name == N && can.Party == PN)
                {
                    can.IncrementVotes();
                    FileStream fin = new FileStream("Candidates.txt", FileMode.Open);
                    StreamWriter writer = new StreamWriter(fin);
                    foreach (Candidate cn in candidates)
                    {
                        writer.WriteLine($"{cn.CandidateID},{cn.Name},{cn.Party},{cn.Votes}");
                    }
                    writer.Flush();
                    writer.Close();
                    fin.Close();

                    string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=infoManager;Integrated Security=True";
                    SqlConnection connection = new SqlConnection(connectionString);
                    connection.Open();
                    string query = "Update Candidate set Votes = @v where CandidateId = @id";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@v",can.Votes);
                    cmd.Parameters.AddWithValue("@id", can.CandidateID);
                    int exe = cmd.ExecuteNonQuery();
                    Console.WriteLine($"{exe} row is changed on Candidate");
                    connection.Close();
                    return true;
                }

            }
            return false;
        }
        public string castVote(Candidate c,Voter v)
        {
            
            foreach (Voter voter in voterList)
            {
                if (v.Cnic == voter.Cnic)
                {
                    if (voter.SelectedPartyName == "Nill" && isCandidateExist(c.Name,c.Party))
                    {

                        voter.SelectedPartyName = c.Party;
                        
                        FileStream fin = new FileStream("VotersDetail.txt", FileMode.Create);
                        StreamWriter writer = new StreamWriter(fin);
                        foreach (Voter vt in voterList)
                        {
                            writer.WriteLine($"{vt.Cnic},{vt.VoterName},{vt.SelectedPartyName}");
                        }
                        writer.Flush();
                        writer.Close();
                        fin.Close();

                        //casting the vote and changing in the database 

                        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=infoManager;Integrated Security=True";
                        SqlConnection connection = new SqlConnection(connectionString);
                        connection.Open();
                        string query = "Update Voter set selectedParty = @Party where voterID=@cnic";
                        SqlCommand cmd = new SqlCommand(query,connection);
                        cmd.Parameters.AddWithValue("@Party", c.Party);
                        cmd.Parameters.AddWithValue("@cnic", v.Cnic);
                        int rowsEffected = cmd.ExecuteNonQuery();
                        Console.WriteLine($"{rowsEffected} rows are changed");
                        connection.Close();
                        return "Vote Casted Successfully ";
                    }
                    else
                    {
                        return "Already given the vote or wrong credentials";
                    }  
                }
            }
            return "Voter with provided Cnic not exist ";
        }


        public void addVoter(string cnic,string name)
        {
            //Ading the voter in the voter list Memory...
            Voter newVoter = new Voter(cnic, name, "Nill");
            voterList.Add(newVoter);
            Console.WriteLine("Voter added successfully");


            //Now Adding the voter in the file system ...
 
            FileStream fin = new FileStream("VotersDetail.txt", FileMode.Append);
            StreamWriter writer= new StreamWriter(fin);
            writer.WriteLine($"{newVoter.Cnic},{newVoter.VoterName},{newVoter.SelectedPartyName}");
            writer.Flush();
            writer.Close();
            fin.Close();


            //Adding the voter in the datbase Table ..

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=infoManager;Integrated Security=True";
            SqlConnection conection = new SqlConnection(connectionString);
            string placeHolder = "Nill";
            conection.Open();
            string query =  "INSERT INTO Voter Values(@cnic,@name,@placeHolder)";
            SqlCommand cmd = new SqlCommand(query, conection);
            cmd.Parameters.AddWithValue("@cnic", cnic);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@placeHolder", placeHolder);
            int rowsEffected = cmd.ExecuteNonQuery();
            Console.WriteLine($"{rowsEffected} rows are inserted ");
            conection.Close();
        }
        public bool UpdateVoter(string cnic)
        {
            foreach (Voter v in voterList)
            {
                if (v.Cnic == cnic)
                {
                    Console.WriteLine("Enter new Voter details: ");
                    Console.Write("Name: ");
                    string name = Console.ReadLine();
                    v.VoterName = name;
                    FileStream fin = new FileStream("VotersDetail.txt", FileMode.Create);
                    StreamWriter writer = new StreamWriter(fin);
                    foreach (Voter vt in voterList)
                    {
                        writer.WriteLine($"{vt.Cnic},{vt.VoterName},{vt.SelectedPartyName}");
                    }
                    writer.Flush();
                    writer.Close();
                    fin.Close();

                    //also updating the voter in the database 
                    string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=infoManager;Integrated Security=True";
                    SqlConnection conection = new SqlConnection(connectionString);
                    conection.Open();
                    string query = "Update Voter set voterName = @Name where voterID = @cnic";
                    SqlCommand cmd = new SqlCommand(query, conection);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@cnic", cnic);
                    int rowsEffected = cmd.ExecuteNonQuery();
                    Console.WriteLine($"{rowsEffected} rows are Updated ");
                    conection.Close();
                    return true;
                }
            }
            return false;
            
        }
        public void displayVoters()
        {
            foreach(Voter v in voterList)
            {
                Console.WriteLine(v.ToString());
            }
        }
        public bool DeleteVoter(string cnic)
        {
            foreach (Voter v in voterList)
            {
                if (v.Cnic == cnic)
                {
                    voterList.Remove(v);
                    FileStream fin = new FileStream("VotersDetail.txt", FileMode.Create);
                    StreamWriter writer = new StreamWriter(fin);
                    foreach(Voter vt in voterList)
                    {
                        writer.WriteLine($"{vt.Cnic},{vt.VoterName},{vt.SelectedPartyName}");
                    }
                    writer.Flush();
                    writer.Close();
                    fin.Close();

                    //deleting the particular voter from database also 
                    string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=infoManager;Integrated Security=True";
                    SqlConnection conection = new SqlConnection(connectionString);
                    conection.Open();
                    string query = "Delete from Voter where voterID = @cnic";
                    SqlCommand cmd = new SqlCommand(query,conection);
                    cmd.Parameters.AddWithValue("@cnic", cnic);
                    int rowsEffected = cmd.ExecuteNonQuery();
                    Console.WriteLine($"{rowsEffected} rows are deleted ");
                    conection.Close();
                    return true;
                }
            }
            return false;
        }
        public void insertCandidate(Candidate c)
        {
            //adding candidate in the Memory  
            candidates.Add(c);
            //Adding Candidate in the File 
            FileStream fin = new FileStream("Candidates.txt", FileMode.Append);
            StreamWriter writer = new StreamWriter(fin);
            writer.WriteLine($"{c.CandidateID},{c.Name},{c.Party},{c.Votes}");
            writer.Flush();
            writer.Close();
            fin.Close() ;
            Console.WriteLine("Candidate inserted ");

            //Inserting candidate in the local disk 
            string conectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=infoManager;Integrated Security=True";
            SqlConnection connection = new SqlConnection(conectionString);
            connection.Open();
            string query = "INSERT into Candidate Values(@candidateId,@Name,@Party,@Votes)";
            SqlCommand cmd = new SqlCommand(query,connection);
            cmd.Parameters.AddWithValue("@candidateId", c.CandidateID);
            cmd.Parameters.AddWithValue("@Name", c.Name);
            cmd.Parameters.AddWithValue("@Party", c.Party);
            cmd.Parameters.AddWithValue("@Votes", c.Votes);
            int rowsChanged = cmd.ExecuteNonQuery();
            Console.WriteLine($"{rowsChanged} rows are effected ");
            connection.Close();
        }

        public void displayCandidates()
        {
            foreach(Candidate c in candidates)
            {
                Console.WriteLine(c.ToString());
            }
        }

        public bool UpdateCandidate(Candidate c , int id)
        {
            foreach(Candidate can in candidates)
            {
                if (can.CandidateID == id)
                {

                    //doing one extra operation in the database that if a candidate
                    //has been deleted from database the voter selected party should
                    //also become null in that case 
                    // first Also Updating in the Main Memory 
                    foreach (Voter v in voterList)
                    {
                        if (v.SelectedPartyName == can.Party)
                        {
                            v.SelectedPartyName = "Nill";
                        }
                    }
                    string conString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=infoManager;Integrated Security=True";
                    SqlConnection conn = new SqlConnection(conString);
                    conn.Open();
                    string dumb = "Nill";
                    string q = "Update Voter set selectedParty=@d where selectedParty=@selectedP";
                    SqlCommand cm = new SqlCommand(q, conn);
                    cm.Parameters.AddWithValue("@d", dumb);
                    cm.Parameters.AddWithValue("@selectedP", can.Party);
                    int ex = cm.ExecuteNonQuery();
                    conn.Close();

                    can.Name = c.Name;
                    can.Party = c.Party;
                    FileStream fin = new FileStream("Candidates.txt", FileMode.Open);
                    StreamWriter writer = new StreamWriter(fin);
                    foreach(Candidate cn in candidates)
                    {
                        writer.WriteLine($"{cn.CandidateID},{cn.Name},{cn.Party},{cn.Votes}");
                    }
                    writer.Flush();
                    writer.Close();
                    fin.Close();



                    //Updating the candidate in the database also 

                    string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=infoManager;Integrated Security=True";
                    SqlConnection connection = new SqlConnection(connectionString);
                    connection.Open();
                    string query = "Update Candidate set Name = @N, Party=@P ,Votes=@V where CandidateId = @id ";
                    SqlCommand cmd = new SqlCommand(query,connection);
                    cmd.Parameters.AddWithValue("@N", c.Name);
                    cmd.Parameters.AddWithValue("@P", c.Party);
                    cmd.Parameters.AddWithValue("@V", 0);
                    cmd.Parameters.AddWithValue("@id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} rows are changed");
                    connection.Close();
                    return true;
                }
            }
            return false;
        }
        public bool DeleteCandidate(int id)
        {
            foreach(Candidate c in candidates)
            {
                if (c.CandidateID==id)
                {
                    candidates.Remove(c);
                    FileStream fin = new FileStream("Candidates.txt", FileMode.Open);
                    StreamWriter writer = new StreamWriter(fin);
                    foreach (Candidate cn in candidates)
                    {
                        writer.WriteLine($"{cn.CandidateID},{cn.Name},{cn.Party},{cn.Votes}");
                    }
                    writer.Flush();
                    writer.Close();
                    fin.Close();

                    //doing one extra operation in the database that if a candidate
                    //has been deleted from database the voter selected party should
                    //also become null in that case 
                    // first Also Updating in the Main Memory 
                    foreach(Voter v in voterList)
                    {
                        if (v.SelectedPartyName == c.Party)
                        {
                            v.SelectedPartyName = "Nill";
                        }
                    }
                    string conString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=infoManager;Integrated Security=True";
                    SqlConnection conn = new SqlConnection (conString);
                    conn.Open();
                    string dumb = "Nill";
                    string q = "Update Voter set selectedParty=@d where selectedParty=@selectedP";
                    SqlCommand cm = new SqlCommand(q,conn);
                    cm.Parameters.AddWithValue("@d", dumb);
                    cm.Parameters.AddWithValue("@selectedP", c.Party);
                    int ex = cm.ExecuteNonQuery();
                    conn.Close();

                    //deleting the candidate from the Databases

                    string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=infoManager;Integrated Security=True";
                    SqlConnection connection = new SqlConnection(connectionString);
                    connection.Open();
                    string query = "Delete from  Candidate where CandidateId = @id ";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row/s is deleted ");
                    connection.Close();





                    return true;
                }
            }
            return false;
        }
        public void DeclareWinner()
        {
            int vote = 0;
            string name="";
            foreach(Candidate c in candidates)
            {
                if (c.Votes > vote)
                {
                    vote = c.Votes;
                    name = c.Name;
                }
            }
            Console.WriteLine("The winner is "+ name + " with votes " + vote);
        }
    }
}
