using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gateway.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Cors;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowOrigin")]  
    public class GatewayController : ControllerBase
    {
       private readonly ILogger<GatewayController> _logger;
        private IConfiguration configuration;


        public GatewayController(ILogger<GatewayController> logger, IConfiguration iconfig)
        {
            _logger = logger;
            configuration = iconfig;
        }

       
        [HttpGet]
        [Authorize]
        [Route("GetLoansBook")]
        public async Task<string> GetLoansBook(string userCode)
        {
             using (var httpClient = new HttpClient())
            {
                List<Loan> loansByUserCode = new List<Loan>();
                string[] bookCodes;
                string loanUrl = configuration.GetSection("MSUrls").GetSection("loanUrl").Value;
                string bookUrl = configuration.GetSection("MSUrls").GetSection("bookUrl").Value;
                loanUrl = loanUrl + "GetLoanByUserCode?userCode=" + userCode;// assigns the url for loans MS
                bookUrl = bookUrl + "GetBooksByBookCodes"; //assigns the url for book MS
                using (var response = await httpClient.GetAsync(loanUrl)) //calls loanMS
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    loansByUserCode = JsonConvert.DeserializeObject<List<Loan>>(apiResponse);
                    bookCodes = loansByUserCode.Select(x => x.BookCode).ToArray();// gets the input for next MS ie BookMS
                }
                var contentData = new StringContent(JsonConvert.SerializeObject(bookCodes), System.Text.Encoding.UTF8, "application/json");  //sets the input data for bookMS
                using (var response = await httpClient.PostAsync(bookUrl, contentData))// calls bookMS
                {
                    var result = response.Content.ReadAsStringAsync().Result;  //gets the result
                    return result; //return the result
                }
            }
        }
   
        
        [HttpGet]
        [Authorize]
        [Route("GetOrderBook")]
        public async Task<string> GetOrderBook(string userCode)
        {
            using (var httpClient = new HttpClient())
            {
                List<Order> orderByUserCode = new List<Order>();
                string[] bookCodes;
                string orderUrl = configuration.GetSection("MSUrls").GetSection("orderUrl").Value;
                string bookUrl = configuration.GetSection("MSUrls").GetSection("bookUrl").Value;
                orderUrl = orderUrl + "GetOrderByUserCode?userCode=" + userCode; //assigns the url for Order MS
                bookUrl = bookUrl + "GetBooksByBookCodes"; //assigns the url for book MS
                using (var response = await httpClient.GetAsync(orderUrl)) //calls loanMS
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    orderByUserCode = JsonConvert.DeserializeObject<List<Order>>(apiResponse);
                    bookCodes = orderByUserCode.Select(x => x.BookCode).ToArray(); //gets the input for next MS ie BookMS
                }
                var contentData = new StringContent(JsonConvert.SerializeObject(bookCodes), System.Text.Encoding.UTF8, "application/json");  //sets the input data for bookMS
                using (var response = await httpClient.PostAsync(bookUrl, contentData)) //calls bookMS
                {
                    var result = response.Content.ReadAsStringAsync().Result; // gets the result
                    return result; //return the result
                }
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetLoanByUserCode")]
        public async Task<ActionResult<List<Loan>>> GetLoanByUserCode(string userCode)
        {
            using (var httpClient = new HttpClient())
            {
                List<Loan> loansByUserCode = new List<Loan>();
                
                string loanUrl = configuration.GetSection("MSUrls").GetSection("loanUrl").Value;
                loanUrl = loanUrl + "GetLoanByUserCode?userCode=" + userCode; //assigns the url for loans MS
                using (var response = await httpClient.GetAsync(loanUrl)) //calls loanMS
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    loansByUserCode = JsonConvert.DeserializeObject<List<Loan>>(apiResponse);
                    return loansByUserCode;
                }
            }
        }

        [Authorize]
        [HttpPost]
        [Route("SaveLoan")]
        public async Task<ActionResult<Loan>> PostLoan(Loan Loan)
        {
            using (var httpClient = new HttpClient())
            {
                string loanUrl = configuration.GetSection("MSUrls").GetSection("loanUrl").Value;
                
                var contentData = new StringContent(JsonConvert.SerializeObject(Loan), System.Text.Encoding.UTF8, "application/json"); // sets the input data for bookMS
                using (var response = await httpClient.PostAsync(loanUrl, contentData)) //calls bookMS
                {
                    var result = response.Content.ReadAsStringAsync().Result; // gets the result
                    return JsonConvert.DeserializeObject<Loan>(result); //return the result
                }
            }
        }

        [Authorize]
        [HttpPost]
        [Route("SaveOrder")]
        public async Task<ActionResult<Order>> PostOrder(Order Order)
        {
            using (var httpClient = new HttpClient())
            {
                string orderUrl = configuration.GetSection("MSUrls").GetSection("orderUrl").Value;
                var contentData = new StringContent(JsonConvert.SerializeObject(Order), System.Text.Encoding.UTF8, "application/json"); // sets the input data for bookMS
                using (var response = await httpClient.PostAsync(orderUrl, contentData)) //calls bookMS
                {
                    var result = response.Content.ReadAsStringAsync().Result; // gets the result
                    return JsonConvert.DeserializeObject<Order>(result); //return the result
                }
            }
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateOrder")]
        public async Task<bool> PutOrder(Order Order)
        {
            using (var httpClient = new HttpClient())
            {
                string orderUrl = configuration.GetSection("MSUrls").GetSection("orderUrl").Value;
                var contentData = new StringContent(JsonConvert.SerializeObject(Order), System.Text.Encoding.UTF8, "application/json"); // sets the input data for bookMS
                using (var response = await httpClient.PutAsync(orderUrl, contentData)) //calls bookMS
                {
                    var result = response.Content.ReadAsStringAsync().Result; // gets the result
                    return Convert.ToBoolean(result); //return the result
                }
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteOrder/{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            using (var httpClient = new HttpClient())
            {
                string orderUrl = configuration.GetSection("MSUrls").GetSection("orderUrl").Value + id; 
                using (var response = await httpClient.DeleteAsync(orderUrl)) //calls bookMS
                {
                    var result = response.Content.ReadAsStringAsync().Result; // gets the result
                    return JsonConvert.DeserializeObject<Order>(result); //return the result
                }
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetOrderByUserCode")]
        public async Task<ActionResult<List<Order>>> GetOrderByUserCode(string userCode)
        {
            using (var httpClient = new HttpClient())
            {
                string orderUrl = configuration.GetSection("MSUrls").GetSection("orderUrl").Value + "GetOrderByUserCode?userCode=" + userCode; 
                using (var response = await httpClient.GetAsync(orderUrl)) //calls loanMS
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Order>>(apiResponse);
                }
            }
        }

        [HttpGet]
        [Route("ValidateUserLogin")]
        //this is the first api which is used for login
        public async Task<IActionResult> ValidateUserLogin(string email, string password)
        {
            using (var httpClient = new HttpClient())
            {
                string userUrl = configuration.GetSection("MSUrls").GetSection("userUrl").Value + "ValidateUserLogin?email=" + email + "&password=" + password;
                using (var response = await httpClient.GetAsync(userUrl)) //calls loanMS
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var userResponse = JsonConvert.DeserializeObject<LoginResponse>(apiResponse);
                    if (userResponse != null && userResponse.IsValid)
                    {
                        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt").GetSection("Key").Value));
                        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                        var tokeOptions = new JwtSecurityToken(
                            claims: new List<Claim>(),
                            expires: DateTime.Now.AddMinutes(30),
                            signingCredentials: signinCredentials
                        );

                        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                        return Ok(new { Token = tokenString, UserCode = userResponse.UserCode,RoleType = userResponse.RoleType, UserId = userResponse.UserId });
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetUsers/{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
            using (var httpClient = new HttpClient())
            {
                string userUrl = configuration.GetSection("MSUrls").GetSection("userUrl").Value + id;
                using (var response = await httpClient.GetAsync(userUrl)) //calls loanMS
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Users>(apiResponse);
                }
            }
        }

        [HttpPost]
        [Route("SaveUser")]
        public async Task<ActionResult<Users>> PostUsers(Users users)
        {
            using (var httpClient = new HttpClient())
            {
                string userUrl = configuration.GetSection("MSUrls").GetSection("userUrl").Value;
                var contentData = new StringContent(JsonConvert.SerializeObject(users), System.Text.Encoding.UTF8, "application/json"); // sets the input data for bookMS
                using (var response = await httpClient.PostAsync(userUrl, contentData)) //calls bookMS
                {
                    var result = response.Content.ReadAsStringAsync().Result; // gets the result
                    return JsonConvert.DeserializeObject<Users>(result); //return the result
                }
            }
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateUser")]
        public async Task<bool> PutUsers(Users users)
        {
            using (var httpClient = new HttpClient())
            {
                string userUrl = configuration.GetSection("MSUrls").GetSection("userUrl").Value;
                var contentData = new StringContent(JsonConvert.SerializeObject(users), System.Text.Encoding.UTF8, "application/json"); // sets the input data for bookMS
                using (var response = await httpClient.PutAsync(userUrl, contentData)) //calls bookMS
                {
                    var result = response.Content.ReadAsStringAsync().Result; // gets the result
                    return Convert.ToBoolean(result); //return the result
                }
            }
        }

        [Authorize]
        [HttpPost]
        [Route("SaveBook")]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            using (var httpClient = new HttpClient())
            {
                string bookUrl = configuration.GetSection("MSUrls").GetSection("bookUrl").Value;
                var contentData = new StringContent(JsonConvert.SerializeObject(book), System.Text.Encoding.UTF8, "application/json"); // sets the input data for bookMS
                using (var response = await httpClient.PostAsync(bookUrl, contentData)) //calls bookMS
                {
                    var result = response.Content.ReadAsStringAsync().Result; // gets the result
                    return JsonConvert.DeserializeObject<Book>(result); //return the result
                }
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetBook")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBook()
        {
            using (var httpClient = new HttpClient())
            {
                string bookUrl = configuration.GetSection("MSUrls").GetSection("bookUrl").Value;
                using (var response = await httpClient.GetAsync(bookUrl)) //calls loanMS
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Book>>(apiResponse);
                }
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetBook/{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            using (var httpClient = new HttpClient())
            {
                string bookUrl = configuration.GetSection("MSUrls").GetSection("bookUrl").Value + id;
                using (var response = await httpClient.GetAsync(bookUrl)) //calls loanMS
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Book>(apiResponse);
                }
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetAllCategories")]
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            using (var httpClient = new HttpClient())
            {
                string bookUrl = configuration.GetSection("MSUrls").GetSection("bookUrl").Value + "GetAllCategories";
                using (var response = await httpClient.GetAsync(bookUrl)) //calls loanMS
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Category>>(apiResponse);
                }
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetBooksByCategoryName")]
        public async Task<string> GetBooksByCategoryName(string name)
        {
            using (var httpClient = new HttpClient())
            {
                string bookUrl = configuration.GetSection("MSUrls").GetSection("bookUrl").Value + "GetBooksByCategoryName?name=" + name;
                using (var response = await httpClient.GetAsync(bookUrl)) //calls loanMS
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return apiResponse;
                }
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetBookByNameAndAuthor")]
        public async Task<ActionResult<List<Book>>> GetBookByNameAndAuthor(string name, string author)
        {
            using (var httpClient = new HttpClient())
            {
                string bookUrl = configuration.GetSection("MSUrls").GetSection("bookUrl").Value + "GetBookByNameAndAuthor?name=" + name + "&author=" + author;
                using (var response = await httpClient.GetAsync(bookUrl)) //calls loanMS
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Book>>(apiResponse);
                }
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetBookByNameOrAuthor")]
        public async Task<ActionResult<List<Book>>> GetBookByNameOrAuthor(string name, string author)
        {
            using (var httpClient = new HttpClient())
            {
                string bookUrl = configuration.GetSection("MSUrls").GetSection("bookUrl").Value + "GetBookByNameOrAuthor?name=" + name + "&author=" + author;
                using (var response = await httpClient.GetAsync(bookUrl)) //calls loanMS
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Book>>(apiResponse);
                }
            }
        }

        [Authorize]
        [HttpGet]
        [Route("SearchBook")]
        public async Task<ActionResult<List<Book>>> SearchBook(string search)
        {
            using (var httpClient = new HttpClient())
            {
                string bookUrl = configuration.GetSection("MSUrls").GetSection("bookUrl").Value + "SearchBook?search=" + search;
                using (var response = await httpClient.GetAsync(bookUrl)) //calls loanMS
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Book>>(apiResponse);
                }
            }
        }

    }
}
