using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class ResetPasswordModelTests : TestBase
{
    public static class HttpContextAccessorMockFactory
    {
        public static IHttpContextAccessor Create(string acceptLanguage)
        {
            var mockHeaders = new Mock<IHeaderDictionary>();
            mockHeaders.Setup(h => h["Accept-Language"]).Returns(new StringValues(acceptLanguage));

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(r => r.Headers).Returns(mockHeaders.Object);

            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(c => c.Request).Returns(mockRequest.Object);

            var mockAccessor = new Mock<IHttpContextAccessor>();
            mockAccessor.Setup(a => a.HttpContext).Returns(mockContext.Object);

            return mockAccessor.Object;
        }
    }

    public ResetPasswordModelTests()
    {
        Services.AddSingleton<IRuleLocalization, DynamicRuleLocalizationTests>();
        Services.AddSingleton<IHttpContextAccessor>(sp => HttpContextAccessorMockFactory.Create("en-US"));
    }

    [Fact]
    public void DefaultValues_Are_EmptyStrings()
    {
        var model = new ResetPasswordModel();

        Assert.Equal(string.Empty, model.Email);
        Assert.Equal(string.Empty, model.Password);
        Assert.Equal(string.Empty, model.ConfirmPassword);
        Assert.Equal(string.Empty, model.Code);
    }

    [Fact]
    public void Properties_SettersAndGetters_Work()
    {
        var model = new ResetPasswordModel
        {
            Email = "user@example.com",
            Password = "securePa$$",
            ConfirmPassword = "securePa$$",
            Code = "code123"
        };

        Assert.Equal("user@example.com", model.Email);
        Assert.Equal("securePa$$", model.Password);
        Assert.Equal("securePa$$", model.ConfirmPassword);
        Assert.Equal("code123", model.Code);
    }

    [Fact]
    public void Email_Has_Required_And_EmailAddress_Attributes()
    {
        var prop = typeof(ResetPasswordModel).GetProperty(nameof(ResetPasswordModel.Email));
        var required = prop?.GetCustomAttributes(typeof(RequiredAttribute), true).FirstOrDefault();
        var emailAttr = prop?.GetCustomAttributes(typeof(EmailAddressAttribute), true).FirstOrDefault();

        Assert.NotNull(prop);
        Assert.NotNull(required);
        Assert.NotNull(emailAttr);
    }

    [Fact]
    public void Password_Has_Required_StringLength_And_DataType_Password()
    {
        var prop = typeof(ResetPasswordModel).GetProperty(nameof(ResetPasswordModel.Password));
        var required = prop?.GetCustomAttributes(typeof(RequiredAttribute), true).FirstOrDefault();
        var strLen = prop?.GetCustomAttributes(typeof(StringLengthAttribute), true).Cast<StringLengthAttribute>().FirstOrDefault();
        var dataType = prop?.GetCustomAttributes(typeof(DataTypeAttribute), true).Cast<DataTypeAttribute>().FirstOrDefault();

        Assert.NotNull(prop);
        Assert.NotNull(required);
        Assert.NotNull(strLen);
        Assert.Equal(100, strLen.MaximumLength);
        Assert.Equal(6, strLen.MinimumLength);
        Assert.NotNull(dataType);
        Assert.Equal(DataType.Password, dataType.DataType);
    }

    [Fact]
    public void ConfirmPassword_Has_Compare_DataTypeAttributes()
    {
        var prop = typeof(ResetPasswordModel).GetProperty(nameof(ResetPasswordModel.ConfirmPassword));
        var compare = prop?.GetCustomAttributes(typeof(CompareAttribute), true).Cast<CompareAttribute>().FirstOrDefault();
        var dataType = prop?.GetCustomAttributes(typeof(DataTypeAttribute), true).Cast<DataTypeAttribute>().FirstOrDefault();

        Assert.NotNull(prop);
        Assert.NotNull(compare);
        Assert.Equal(nameof(ResetPasswordModel.Password), compare.OtherProperty);
        Assert.NotNull(dataType);
        Assert.Equal(DataType.Password, dataType.DataType);
    }

    [Fact]
    public void Code_Has_RequiredAttribute()
    {
        var prop = typeof(ResetPasswordModel).GetProperty(nameof(ResetPasswordModel.Code));
        var required = prop?.GetCustomAttributes(typeof(RequiredAttribute), true).FirstOrDefault();

        Assert.NotNull(prop);
        Assert.NotNull(required);
    }

    [Fact]
    public void Validation_Succeeds_With_Valid_Model()
    {
        var model = new ResetPasswordModel
        {
            Email = "user@example.com",
            Password = "Thi$IsPa55w0rd",
            ConfirmPassword = "Thi$IsPa55w0rd",
            Code = "abc"
        };

        var results = ValidateModel(model);

        Assert.Empty(results);
    }

    [Fact]
    public void Validation_Fails_When_Passwords_DoNot_Match()
    {
        var model = new ResetPasswordModel
        {
            Email = "user@example.com",
            Password = "strongpass",
            ConfirmPassword = "otherpass",
            Code = "abc"
        };

        var results = ValidateModel(model);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(ResetPasswordModel.ConfirmPassword)));
    }

    [Fact]
    public void Validation_Fails_When_Email_Is_Invalid()
    {
        var model = new ResetPasswordModel
        {
            Email = "not-an-email",
            Password = "strongpass",
            ConfirmPassword = "strongpass",
            Code = "abc"
        };

        var results = ValidateModel(model);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(ResetPasswordModel.Email)));
    }

    [Fact]
    public void Validation_Fails_When_Password_Too_Short()
    {
        var model = new ResetPasswordModel
        {
            Email = "user@example.com",
            Password = "short",
            ConfirmPassword = "short",
            Code = "abc"
        };

        var results = ValidateModel(model);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(ResetPasswordModel.Password)));
    }

    [Fact]
    public void Validation_Fails_When_Required_Fields_Are_Empty()
    {
        var model = new ResetPasswordModel(); // all empty

        var results = ValidateModel(model);

        // Expect at least Email, Password and Code required errors
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(ResetPasswordModel.Email)));
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(ResetPasswordModel.Password)));
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(ResetPasswordModel.Code)));
    }

    private  IList<ValidationResult> ValidateModel(object model)
    {
        var ctx = new ValidationContext(model, serviceProvider: Services, items: null);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, ctx, results, validateAllProperties: true);
        return results;
    }
}
