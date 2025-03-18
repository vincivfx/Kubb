using System.ComponentModel.DataAnnotations;

public class SendJolly 
{
    [Required]
    public Guid TeamId {get;set;}
    
    [Required]
    public int QuestionId {get;set;}
}