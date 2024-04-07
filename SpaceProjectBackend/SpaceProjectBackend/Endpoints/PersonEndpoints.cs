using Microsoft.AspNetCore.Mvc;
using SpaceProjectBackend.Repository;
using SpaceProjectBackend.Models;
using Microsoft.AspNetCore.Authorization;
using SpaceProjectBackend.DTOs;
using SpaceProjectBackend.Enums;
using System.Security.Claims;
using SpaceProjectBackend.Utils;

namespace SpaceProjectBackend.Endpoints
{
    public static class PersonEndpoints
    {
        public static void ConfigurePersonEndpoints(this WebApplication app)
        {
            var PersonGroup = app.MapGroup("people");

            PersonGroup.MapPost("/", CreatePerson);
            PersonGroup.MapGet("/", GetPeople);
            PersonGroup.MapGet("/{Id}", GetPerson);
            PersonGroup.MapPut("/{Id}", UpdatePerson);
            PersonGroup.MapDelete("/{Id}", DeletePerson);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static async Task<IResult> CreatePerson(
            PersonPostPayload payload,
            IPersonRepository PersonRepository, 
            IAuthRepository authRepository,
            ClaimsPrincipal user)
        {

            var uid = user.UserId();

            if (uid == null)
            {
                return Results.Unauthorized();
            }
            
            if (payload.Name== "" || payload.Image == "" || payload.Description == "")
            {
                return Results.BadRequest("Non-empty fields are required");
            }

            if (payload.Name== null || payload.Description == null || payload.Image == null )
            {
                return Results.BadRequest("Non-null fields are required");
            }

            Person? Person = await PersonRepository.CreatePerson(payload.Name, payload.Image, payload.Real, payload.Description);

            if (Person == null)
            {
                return Results.BadRequest("Failed to create Person.");
            }

          
            PersonRepository.SaveChanges();

            PersonSingleDTO mdto = new PersonSingleDTO(Person);

            return TypedResults.Created($"/Persons/{Person.Id}", mdto);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize()]
        public static async Task<IResult> GetPeople(IPersonRepository repository)
        {

            var res = await repository.GetPeople();

            List<PersonFullDTO> reslist = new List<PersonFullDTO>();

            foreach (Person prs in res)
            {
                PersonFullDTO dt = new PersonFullDTO(prs);

                reslist.Add(dt);
            }

            return TypedResults.Ok(reslist);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static async Task<IResult> UpdatePerson(string PersonId, PersonUpdatePayload payload, IPersonRepository repository, ClaimsPrincipal user)
        {

            if (payload.Name== null || payload.Description == null || payload.Image == null)
            {
                return Results.BadRequest("Non-null fields are required");
            }


            Person? ogPerson = await repository.GetPerson(PersonId, PreloadPolicy.PreloadRelations);

            if (ogPerson == null)
            {
                return Results.BadRequest("Person not found");
            }

            string newName = (payload.Name.Length > 0) ? payload.Name: ogPerson.Name;

            string newImage = (payload.Image.Length > 0) ? payload.Image : ogPerson.Image;

            string newProfile = (payload.Description.Length > 0) ? payload.Description : ogPerson.Description;

            bool newReal = ogPerson.Real;

            if (payload.Real != null && newReal != payload.Real)
            {
                newReal = (bool)payload.Real;
            }


            Person? Person = await repository.UpdatePerson(ogPerson.Id, newName, newImage, newReal, newProfile);

            if (Person == null)
            {
                return Results.BadRequest("Failed to create Message.");
            }


            PersonSingleDTO mv = new PersonSingleDTO(Person);


            repository.SaveChanges();

            return TypedResults.Created($"/Persons/{Person.Id}", mv);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static async Task<IResult> GetPerson(string PersonId, IPersonRepository repository)
        {

            Person? Person = await repository.GetPerson(PersonId, PreloadPolicy.PreloadRelations);

            if (Person == null)
            {
                return Results.NotFound("Person not found");
            }

            var PersonDTO = new PersonFullDTO(Person);

            repository.SaveChanges();

            return TypedResults.Ok(PersonDTO);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static async Task<IResult> DeletePerson(string PersonId, IPersonRepository repository, ClaimsPrincipal user)
        {

            Person? Person = await repository.DeletePerson(PersonId, PreloadPolicy.PreloadRelations);

            if (Person == null)
            {
                return Results.NotFound("Person not found");
            }

            var PlayerDTO = new PersonSingleDTO(Person);

            repository.SaveChanges();

            return TypedResults.Ok(PlayerDTO);
        }
    }
}
