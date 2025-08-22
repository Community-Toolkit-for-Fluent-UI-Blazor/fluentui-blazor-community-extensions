using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Cookies;

public class CookieDataTests
{
    // Helper method to create a sample list of CookieItems
    private List<CookieItem> CreateSampleItems()
    {
        return
        [
            new CookieItem
            {
                Name = "essential",
                Title = "Essential Cookies",
                IsActive = true
            },
            new CookieItem
            {
                Name = "analytics",
                Title = "Analytics Cookies",
                IsActive = false
            },
            new CookieItem
            {
                Name = "marketing",
                Title = "Marketing Cookies",
                IsActive = false
            }
        ];
    }

    [Fact]
    public void CookieData_CanBeInstantiatedAndPropertiesAccessed()
    {
        // Arrange
        var items = CreateSampleItems();
        var labels = CookieLabels.Default;

        // Act
        var cookieData = new CookieData(items, labels);

        // Assert
        Assert.NotNull(cookieData.Items);
        Assert.Equal(cookieData.Items.Count(), items.Count);
        Assert.Equal(cookieData.Items, items);
        Assert.NotNull(cookieData.Labels);
        Assert.Equal(cookieData.Labels, labels);
        Assert.Equal(cookieData.Labels.Title, CookieLabels.Default.Title);
    }

    [Fact]
    public void CookieData_HandlesEmptyItemsCollection()
    {
        // Arrange
        var items = new List<CookieItem>();
        var labels = CookieLabels.Default;

        // Act
        var cookieData = new CookieData(items, labels);

        // Assert
        Assert.NotNull(cookieData.Items);
        Assert.Empty(cookieData.Items);
        Assert.Equal(cookieData.Labels, labels);
    }


    [Fact]
    public void CookieData_ValueEquality_ReturnsTrueForIdenticalRecords()
    {
        // Arrange
        var items1 = CreateSampleItems();
        var labels1 = CookieLabels.Default;
        var data1 = new CookieData(items1, labels1);

        // Create new instances with identical values
        var items2 = CreateSampleItems();
        var labels2 = CookieLabels.Default;
        var data2 = new CookieData(items2, labels2);

        // Act & Assert
        Assert.Equal(data1.Items, data2.Items);
        Assert.Same(data1.Labels, data2.Labels);
    }

    [Fact]
    public void CookieData_ValueEquality_ReturnsFalseForDifferentRecords()
    {
        // Arrange
        var items1 = CreateSampleItems();
        var labels1 = CookieLabels.Default;
        var data1 = new CookieData(items1, labels1);

        // Create a record with different items
        var items2 = new List<CookieItem> { new CookieItem
        {
            Name = "One",
            Title = "One",
        }
        };
        var data2 = new CookieData(items2, labels1);

        // Create a record with different labels
        var labels3 = CookieLabels.French;
        var data3 = new CookieData(items1, labels3);

        // Act & Assert
        Assert.NotEqual(data1, data2);
        Assert.NotEqual(data1.GetHashCode(), data2.GetHashCode()); 

        Assert.NotEqual(data1, data3);
        Assert.NotEqual(data1.GetHashCode(), data3.GetHashCode());
    }

    [Fact]
    public void CookieData_Deconstruction_WorksCorrectly()
    {
        // Arrange
        var items = CreateSampleItems();
        var labels = CookieLabels.Default;
        RenderFragment<CookieItem>? fragment = i => __builder => { };
        var cookieData = new CookieData(items, labels, fragment);

        // Act
        var (deconstructedItems, deconstructedLabels, decontructedFragment) = cookieData;

        // Assert
        Assert.Equal(deconstructedItems, items);
        Assert.Equal(deconstructedLabels, labels);
        Assert.Equal(decontructedFragment, fragment);
    }

    [Fact]
    public void CookieData_WithExpression_CreatesNewRecordWithModifiedProperty()
    {
        // Arrange
        var originalItems = CreateSampleItems();
        var originalLabels = CookieLabels.Default;
        var originalData = new CookieData(originalItems, originalLabels);

        var newLabels = CookieLabels.French;

        // Act
        var updatedData = originalData with { Labels = newLabels };

        // Assert
        Assert.NotSame(updatedData, originalData);
        Assert.NotEqual(updatedData, originalData); 

        Assert.Equal(updatedData.Labels, newLabels); 
        Assert.Equal(updatedData.Items, originalItems);
        Assert.Same(updatedData.Items, originalItems); 
    }

    [Fact]
    public void CookieData_WithExpression_CreatesNewRecordWithModifiedItems()
    {
        // Arrange
        var originalItems = CreateSampleItems();
        var originalLabels = CookieLabels.Default;
        var originalData = new CookieData(originalItems, originalLabels);

        var newItems = new List<CookieItem> { new CookieItem
        {
            Name = "new",
            Title = "new",
        }
        };

        // Act
        var updatedData = originalData with { Items = newItems };

        // Assert
        Assert.NotSame(updatedData, originalData); 
        Assert.NotEqual(updatedData, originalData); 

        Assert.Equal(updatedData.Items, newItems); 
        Assert.Equal(updatedData.Labels, originalLabels);
        Assert.Same(updatedData.Labels, originalLabels);
    }
}
