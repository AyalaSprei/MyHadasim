using CoronaClinic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CoronaClinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MinimalApiController : ControllerBase
    {
        private readonly CoronaClinicContext _coronaClinicContext;
        private readonly IMapper _mapper;
        public MinimalApiController(CoronaClinicContext coronaClinicContext, IMapper mapper)
        {
            _coronaClinicContext = coronaClinicContext;
            _mapper = mapper;
        }

        // GET: api/MinimalApi
        [HttpGet]
        public async Task<ActionResult<List<MemberMinimal>>> GetAllMembers()
        {
            List<MemberMinimal> members = await _coronaClinicContext.Members
                .AsNoTracking()
                .Select(m => new MemberMinimal { Id = m.Id, Name = m.Name, LastName = m.LastName })
                .ToListAsync();

            return Ok(members);
        }

        // GET: api/MinimalApi/id
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>> GetMemberById(int id)
        {
            // Retrieve member from the database including related entities
            var member = await _coronaClinicContext.Members
                .Include(m => m.Immunes)
                    .ThenInclude(i => i.Creator)
                .Include(m => m.Illnesses)
                .FirstOrDefaultAsync(m => m.Id == id);

            // Check if member exists
            if (member == null)
            {
                return NotFound();
            }

            // Map member entity to MemberDto
            var memberDto = _mapper.Map<MemberDto>(member);

            // Map Illnesses to MemberDto properties
            var illness = member.Illnesses.FirstOrDefault();
            if (illness != null)
            {
                memberDto.IllnessId = illness.IllnessId;
                memberDto.PositiveDate = illness.PositiveDate;
                memberDto.NegativeDate = illness.NegativeDate;
            }

            // Map Immunes to ImmuneDtos
            memberDto.Immunes = _mapper.Map<List<ImmuneDto>>(member.Immunes);

            // Return member DTO
            return Ok(memberDto);
        }


        [HttpPost("CreateMemberAndRecords")]
        public async Task<ActionResult> CreateMemberAndRecords( [FromBody] MemberDto memberDto)
        {
            //string picturePath = null;

            //// Upload picture if provided
            //if (profilePictureFile != null)
            //{
            //    try
            //    {
            //        if (profilePictureFile.Length > 0)
            //        {
            //            // Define unique and secure file path
            //            var fileName = Path.GetRandomFileName() + Path.GetExtension(profilePictureFile.FileName);
            //            var filePath = Path.Combine(GetUploadDirectoryPath(), fileName); // Use a dedicated upload directory

            //            // Save the file to the server
            //            using (var stream = new FileStream(filePath, FileMode.Create))
            //            {
            //                await profilePictureFile.CopyToAsync(stream);
            //            }

            //            picturePath = filePath;
            //            memberDto.Picture = picturePath;
            //        }
            //        else
            //        {
            //            // Handle no file upload case (optional)
            //            // memberDto.Picture = someDefaultPicturePath; // Or leave null
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        // Log and handle upload errors gracefully
            //        throw new Exception("Error uploading picture."); // Rethrow for client-side handling
            //    }
            //}

            var member = new Member
            {
                Name = memberDto.Name,
                LastName = memberDto.LastName,
                IdentityNumber = memberDto.IdentityNumber,
                City = memberDto.City,
                Street = memberDto.Street,
                HomeNumber = memberDto.HomeNumber,
                Birthdate = memberDto.Birthdate,
                Telephone = memberDto.Telephone,
                MobilePhone = memberDto.MobilePhone,
                IsImmune = memberDto.IsImmune
            };

            await _coronaClinicContext.Members.AddAsync(member);
            await _coronaClinicContext.SaveChangesAsync();

            int memberId = member.Id;

            if (memberDto.Immunes != null)
            {
                foreach (var immuneDto in memberDto.Immunes)
                {
                    if (immuneDto.CreatorId == 0)
                    {
                        break;
                    }
                    var immune = new Immune
                    {
                        MemberId = memberId,
                        Date = immuneDto.Date,
                        CreatorId = immuneDto.CreatorId
                    };
                    await _coronaClinicContext.Immunes.AddAsync(immune);
                }
            }

            if (memberDto.PositiveDate != null)
            {
                var illness = new Illness
                {
                    MemberId = memberId,
                    PositiveDate = memberDto.PositiveDate,
                    NegativeDate = memberDto.NegativeDate
                };
                await _coronaClinicContext.Illnesses.AddAsync(illness);

            }
            await _coronaClinicContext.SaveChangesAsync();

            return Ok("Member and associated records created successfully");
        }

        [HttpPut("UpdateMemberAndRecords/{id}")]
        public async Task<ActionResult> UpdateMemberAndRecords(int id, MemberDto memberDto)
        {
            var member = await _coronaClinicContext.Members.FirstOrDefaultAsync(m => m.Id == id);

            if (member == null)
            {
                return NotFound("Member not found");
            }

            // Update Member entity
            member.Name = memberDto.Name;
            member.LastName = memberDto.LastName;
            member.IdentityNumber = memberDto.IdentityNumber;
            member.City = memberDto.City;
            member.Street = memberDto.Street;
            member.HomeNumber = memberDto.HomeNumber;
            member.Birthdate = memberDto.Birthdate;
            member.Telephone = memberDto.Telephone;
            member.MobilePhone = memberDto.MobilePhone;
            member.IsImmune = memberDto.IsImmune;

            // Update Immune records
            var existingImmunes = _coronaClinicContext.Immunes.Where(i => i.MemberId == id).ToList();
            _coronaClinicContext.Immunes.RemoveRange(existingImmunes); // Remove existing immunes
            await _coronaClinicContext.SaveChangesAsync();

            if (memberDto.Immunes != null)
            {
                foreach (var immuneDto in memberDto.Immunes)
                {
                    if (immuneDto.CreatorId == 0)
                    {
                        break;
                    }
                    var immune = new Immune
                    {
                        MemberId = id,
                        Date = immuneDto.Date,
                        CreatorId = immuneDto.CreatorId
                    };
                    if (CheckImmuneCountForMember(id))
                    {
                       await _coronaClinicContext.Immunes.AddAsync(immune);
                       await _coronaClinicContext.SaveChangesAsync();
                    }
                }
            }

            // Update Illness record
            var existingIllness = _coronaClinicContext.Illnesses.FirstOrDefault(i => i.MemberId == id);
            if (existingIllness != null)
            {
                _coronaClinicContext.Illnesses.Remove(existingIllness); // Remove existing illness
            }

            if (memberDto.PositiveDate!=null)
            {
                var illness = new Illness
                {
                    MemberId = id,
                    PositiveDate = memberDto.PositiveDate,
                    NegativeDate = memberDto.NegativeDate
                };
               await  _coronaClinicContext.Illnesses.AddAsync(illness); // Add new illness
            }

           await _coronaClinicContext.SaveChangesAsync(); // Save changes

            return Ok("Member and associated records updated successfully");
        }
        private bool CheckImmuneCountForMember(int memberId)
        {
            var immuneCount = _coronaClinicContext.Immunes.Count(i => i.MemberId == memberId);
            if (immuneCount < 4) {
                return true;
            }
            return false;   
           
        }
        [HttpDelete("DeleteMemberAndRecords/{memberId}")]
        public async Task<ActionResult> DeleteMember(int memberId)
        {
            // Remove related records from Illnesses table
            var illnesses = await _coronaClinicContext.Illnesses.Where(i => i.MemberId == memberId).ToListAsync();
            _coronaClinicContext.Illnesses.RemoveRange(illnesses);

            // Remove related records from Immunes table
            var immunes = await _coronaClinicContext.Immunes.Where(i => i.MemberId == memberId).ToListAsync();
            _coronaClinicContext.Immunes.RemoveRange(immunes);

            await _coronaClinicContext.SaveChangesAsync();
            var member = await _coronaClinicContext.Members.FindAsync(memberId);

            if (member == null)
            {
                return NotFound();
            }

            _coronaClinicContext.Members.Remove(member);

            await _coronaClinicContext.SaveChangesAsync();
            return Ok("Member and associated records deleted successfully");
        }
        [HttpGet("getNotImmunedCount")]
        public async Task<ActionResult<int>> GetNotImmunedCount()
        {
            try
            {
                int count = await _coronaClinicContext.Members.CountAsync(member => !member.IsImmune);
                return Ok(count);
            }
            catch (Exception ex) {
                return NoContent();

            }
            
        }
        [HttpGet("getIllsPerDay")]
        public async Task<ActionResult<List<int>>> GetIllsPerDay(int month)
        {
            var startDate = new DateTime(DateTime.Now.Year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            // Query illnesses that have positive date or negative date within the given month
            IEnumerable<Illness> illnessesInMonth = await _coronaClinicContext.Illnesses
                .Where(i => i.PositiveDate >= startDate && i.PositiveDate <= endDate ||
                            i.NegativeDate >= startDate && i.NegativeDate <= endDate)
                .ToListAsync();
            int[] numberPerDay = new int[DateTime.DaysInMonth(DateTime.Now.Year, month)];

            foreach (var illness in illnessesInMonth)
            {
                DateTime start = illness.PositiveDate > startDate ? illness.PositiveDate : startDate;
                DateTime end = illness.NegativeDate < endDate ? illness.NegativeDate : endDate;

                for (DateTime date = start; date <= end; date = date.AddDays(1))
                {
                    int dayOfMonth = date.Day;
                    numberPerDay[dayOfMonth - 1]++;
                }
            }
            return Ok(numberPerDay);

        }
        private string GetUploadDirectoryPath()
        {
            // Configure a dedicated upload directory outside the wwwroot folder
            // for security reasons. This path should be configurable
            // or determined based on your application's architecture.
            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }
            return uploadDirectory;
        }
    }
    }