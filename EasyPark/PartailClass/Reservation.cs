using EasyPark.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace EasyPark.Models

{
    [ModelMetadataType(typeof(ReservationMetadata))]
    public partial class Reservation
    {
    }
}
