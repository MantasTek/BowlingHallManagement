using BowlingHallManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingHallManagement.Factories
{
    /*
     * Factory Pattern Implementation for Members class
     * 
     * The Factory Pattern is a creational design pattern that provides an interface
     * for creating objects without specifying their concrete classes.
    */
    
    public class MemberFactory
    {
        private static int nextId =1;

        public static Member CreateMember(string name, string email)
        {
            //Validating inputs
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("This Field cannot be empty");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("This Field cannot be empty");

            //Creating a new Member object with random ID
            var member = new Member(nextId, name, email);
            nextId++;

            return member;
        }
    }
}
