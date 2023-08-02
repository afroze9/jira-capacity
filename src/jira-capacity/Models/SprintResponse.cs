namespace jira_capacity.Models;

public class SprintResponse
{
    public int Id { get; set; }
    public string Self { get; set; }
    public string State { get; set; }
    public string Name { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public int OriginBoardId { get; set; }
    public string Goal { get; set; }
}