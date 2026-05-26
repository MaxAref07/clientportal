using ClientPortal.Domain.Enums;

namespace ClientPortal.WebAPI.Models;

public class ChangeFeatureStatusRequest
{
    public FeatureStatus NewStatus { get; set; }
}