using EasyPark.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace EasyPark.Models
{
    [ModelMetadataType(typeof(CustomerMetadata))]
    public partial class Customer
    {
    }
}

