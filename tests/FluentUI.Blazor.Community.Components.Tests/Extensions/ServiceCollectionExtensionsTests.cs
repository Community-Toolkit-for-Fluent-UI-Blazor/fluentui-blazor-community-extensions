using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Internal;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddFluentCxUIComponents_RegistersDropZoneState()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddFluentCxUIComponents();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var dropZoneState = serviceProvider.GetService<DropZoneState<string>>();
        Assert.NotNull(dropZoneState);
    }

    [Fact]
    public void AddFluentCxUIComponents_RegistersFileManagerState()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddFluentCxUIComponents();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var fileManagerState = serviceProvider.GetService<FileManagerState>();
        Assert.NotNull(fileManagerState);
    }

    [Fact]
    public void AddFluentCxUIComponents_RegistersDeviceInfoState()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddFluentCxUIComponents();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var deviceInfoState = serviceProvider.GetService<DeviceInfoState>();
        Assert.NotNull(deviceInfoState);
    }

    [Fact]
    public void AddFluentCxUIComponents_ReturnsServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddFluentCxUIComponents();

        // Assert
        Assert.Same(services, result);
    }

    [Fact]
    public void AddFluentCxUIComponents_RegistersServicesAsScoped()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddFluentCxUIComponents();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        
        // Test that scoped services return same instance within scope
        using var scope1 = serviceProvider.CreateScope();
        var fileManagerState1a = scope1.ServiceProvider.GetService<FileManagerState>();
        var fileManagerState1b = scope1.ServiceProvider.GetService<FileManagerState>();
        Assert.Same(fileManagerState1a, fileManagerState1b);

        var deviceInfoState1a = scope1.ServiceProvider.GetService<DeviceInfoState>();
        var deviceInfoState1b = scope1.ServiceProvider.GetService<DeviceInfoState>();
        Assert.Same(deviceInfoState1a, deviceInfoState1b);

        // Test that different scopes get different instances
        using var scope2 = serviceProvider.CreateScope();
        var fileManagerState2 = scope2.ServiceProvider.GetService<FileManagerState>();
        var deviceInfoState2 = scope2.ServiceProvider.GetService<DeviceInfoState>();
        
        Assert.NotSame(fileManagerState1a, fileManagerState2);
        Assert.NotSame(deviceInfoState1a, deviceInfoState2);
    }

    [Fact]
    public void AddFluentCxUIComponents_RegistersGenericDropZoneState()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddFluentCxUIComponents();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        
        // Test different generic types
        var dropZoneStateString = serviceProvider.GetService<DropZoneState<string>>();
        var dropZoneStateInt = serviceProvider.GetService<DropZoneState<int>>();
        var dropZoneStateObject = serviceProvider.GetService<DropZoneState<object>>();
        
        Assert.NotNull(dropZoneStateString);
        Assert.NotNull(dropZoneStateInt);
        Assert.NotNull(dropZoneStateObject);
        
        // Each generic type should be a different instance
        Assert.NotSame(dropZoneStateString, dropZoneStateInt);
        Assert.NotSame(dropZoneStateString, dropZoneStateObject);
        Assert.NotSame(dropZoneStateInt, dropZoneStateObject);
    }

    [Fact]
    public void AddFluentCxUIComponents_CanBeCalledMultipleTimes()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act - Call multiple times to ensure no conflicts
        services.AddFluentCxUIComponents();
        services.AddFluentCxUIComponents();
        services.AddFluentCxUIComponents();

        // Assert - Should still work correctly
        var serviceProvider = services.BuildServiceProvider();
        var fileManagerState = serviceProvider.GetService<FileManagerState>();
        var deviceInfoState = serviceProvider.GetService<DeviceInfoState>();
        var dropZoneState = serviceProvider.GetService<DropZoneState<string>>();
        
        Assert.NotNull(fileManagerState);
        Assert.NotNull(deviceInfoState);
        Assert.NotNull(dropZoneState);
    }

    [Fact]
    public void AddFluentCxUIComponents_AddsServicesEvenWhenExistingServicesPresent()
    {
        // Arrange
        var services = new ServiceCollection();
        var customFileManagerState = new FileManagerState();
        services.AddSingleton(customFileManagerState);

        // Act
        services.AddFluentCxUIComponents();

        // Assert - Should have both services registered
        var serviceProvider = services.BuildServiceProvider();
        var allFileManagerStates = serviceProvider.GetServices<FileManagerState>().ToList();
        
        // Should have both the singleton and the scoped registration
        Assert.Equal(2, allFileManagerStates.Count);
        Assert.Contains(customFileManagerState, allFileManagerStates);
    }
}