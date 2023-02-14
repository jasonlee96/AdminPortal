﻿namespace AdminPortal.Business.Domains
{
    public class UserAuthenticationRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class UserAuthenticationResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}