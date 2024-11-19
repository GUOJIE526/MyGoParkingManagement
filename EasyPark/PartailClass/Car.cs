using EasyPark.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace EasyPark.Models
{
    [ModelMetadataType(typeof(CarMetadata))]
    public partial class Car
    {
    }
}
