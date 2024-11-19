﻿using EasyPark.Models;
using System.ComponentModel.DataAnnotations;

namespace EasyPark.PartailClass
{
    public class CarExistsAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            // 獲取 MyGoParkingContext 實例
            var context = (EasyParkContext)validationContext.GetService(typeof(EasyParkContext));
            if (context == null)
            {
                return new ValidationResult("資料庫上下文無法使用。");
            }


            var carPlateNumber = value?.ToString();



            // 查詢車牌號碼是否存在
            var carExists = context.Car.Any(car => car.LicensePlate == carPlateNumber);

            if (!carExists)
            {
                return new ValidationResult("此車牌號碼未註冊.");
            }

            return ValidationResult.Success;
        }
    }
}
