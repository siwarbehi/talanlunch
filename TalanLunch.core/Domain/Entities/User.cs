﻿using System.ComponentModel.DataAnnotations;
using TalanLunch.Core.Domain.Enums;
using TalanLunch.Core.Domain.Entities;
using TalanLunch.core.Domain.Entities;

namespace TalanLunch.Core.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; } 
        public string PhoneNumber { get; set; } = string.Empty;
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;  
        public UserRole UserRole { get; set; }  
        public byte[]? ProfilePicture { get; set; } 
        public string FirstName { get; set; } = string.Empty;  
        public string LastName { get; set; } = string.Empty;  
        public string HashedPassword { get; set; } = string.Empty;  

       
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<DishRating> DishRatings { get; set; } = new List<DishRating>();
    }
}
