﻿using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Features.Authentication;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Discussion.Web.Tests.IntegrationTests
{
    [Collection("AppSpecs")]
    public class TopicRelatedPagesSpecs
    {
        
        private Application _theApp;
        public TopicRelatedPagesSpecs(Application theApp)
        {
            _theApp = theApp;
        }



        [Fact]
        public async Task should_serve_topic_list_page()
        {
            // arrange
            var request = _theApp.Server.CreateRequest("/topic/list");

            // act
            var response = await request.GetAsync();

            // assert
            response.StatusCode.ShouldEqual(HttpStatusCode.OK);
        }


        [Fact]
        public async Task should_serve_create_topic_page()
        {
            // arrange
            var request = _theApp.Server.CreateRequest("/topic/create");

            var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, 15.ToString(), ClaimValueTypes.Integer32),
                    new Claim(ClaimTypes.Name, "Hehe", ClaimValueTypes.String),
                    new Claim("SigninTime", System.DateTime.UtcNow.Ticks.ToString(), ClaimValueTypes.Integer64)
                };
            var identity = new ClaimsIdentity(claims, "Cookies");
            _theApp.User = new ClaimsPrincipal(identity);

            // act
            var response = await request.GetAsync();

            // assert
            response.StatusCode.ShouldEqual(HttpStatusCode.OK);
        }


        [Fact]
        public async Task should_redirect_to_signin_when_access_create_topic_page_without_user_principal()
        {
            // arrange
            var request = _theApp.Server.CreateRequest("/topic/create");

            // act
            var response = await request.GetAsync();

            // assert
            response.StatusCode.ShouldEqual(HttpStatusCode.Redirect);
            response.Headers.Location.ToString().Contains("signin").ShouldEqual(true);
        }


        [Fact]
        public async Task should_accept_create_topic_request_with_valid_post_data()
        {
            // arrange
            var request = _theApp.Server.CreateRequest("/topic/createtopic");
            request.And(req =>
            {
                req.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"title", "中文的 title" },
                    {"content", "some content" },
                });
            });

            // act
            var response = await request.PostAsync();

            // assert
            response.StatusCode.ShouldEqual(HttpStatusCode.Redirect);
        }

        [Fact]
        public async Task should_not_accept_create_topic_request_with_invalid_post_data()
        {
            // arrange
            var request = _theApp.Server.CreateRequest("/topic/createtopic");

            // act
            var response = await request.PostAsync();

            // assert
            response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
        }
        
    }
}
