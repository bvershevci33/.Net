namespace AdminPandel.Models
{
    public class PersonImg
    {
        public int Id { get; set; }
        public string Path { get; set; }

        public Profesor Profesor { get; set; }

        public int ProfesorId { get; set; }

    }
}
