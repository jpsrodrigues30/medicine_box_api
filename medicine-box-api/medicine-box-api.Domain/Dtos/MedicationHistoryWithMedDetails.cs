namespace medicine_box_api.Domain.Dtos;
public class MedicationHistoryWithMedDetails
{
    public string MedicationName { get; set; } = string.Empty;
    public MedicationHistoryDetails History { get; set; } = new MedicationHistoryDetails(); 
}
