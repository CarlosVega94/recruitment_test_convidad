using LibraryDatabase.Entitites;
using MediatR;
using System;
using System.Collections.Generic;

namespace LibraryDatabase.Authors.Commands.CreateAuthor

{
    public class CreateAuthorCommand : IRequest<int>
    {
        public CreateAuthorCommand(
            string name,
            string nationality,
            DateTime birthDate,
            IReadOnlyCollection<Book> books)
        { 
            Name = name;
            Nationality = nationality;
            BirthDate = birthDate;
        }
        public string Name { get; }
        public string Nationality { get; }
        public DateTime BirthDate { get; }
    }

}
