global using System.Reflection;

global using AutoMapper;

global using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
global using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
global using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;
global using ECommerce_Clean_Arch.Application.Abstractions.Services;
global using ECommerce_Clean_Arch.Application.Authentication.Common;
global using ECommerce_Clean_Arch.Application.Authentication.Services;
global using ECommerce_Clean_Arch.Application.Categories.Queries.Common;
global using ECommerce_Clean_Arch.Application.Common.Behaviors;
global using ECommerce_Clean_Arch.Application.Common.Interfaces;
global using ECommerce_Clean_Arch.Application.Common.Models;
global using ECommerce_Clean_Arch.Application.Orders.Commands.Checkout.Dtos;
global using ECommerce_Clean_Arch.Application.Orders.Queries.GetById;
global using ECommerce_Clean_Arch.Application.Products.Common;
global using ECommerce_Clean_Arch.Application.Products.Queries.Common;
global using ECommerce_Clean_Arch.Domain.Carts;
global using ECommerce_Clean_Arch.Domain.Categories;
global using ECommerce_Clean_Arch.Domain.Categories.ValueObjects;
global using ECommerce_Clean_Arch.Domain.Common.Interfaces;
global using ECommerce_Clean_Arch.Domain.Errors.Categories;
global using ECommerce_Clean_Arch.Domain.Errors.Orders;
global using ECommerce_Clean_Arch.Domain.Errors.Products;
global using ECommerce_Clean_Arch.Domain.Errors.Security;
global using ECommerce_Clean_Arch.Domain.Errors.Token;
global using ECommerce_Clean_Arch.Domain.Errors.Users;
global using ECommerce_Clean_Arch.Domain.Orders;
global using ECommerce_Clean_Arch.Domain.Orders.Entities;
global using ECommerce_Clean_Arch.Domain.Orders.Enums;
global using ECommerce_Clean_Arch.Domain.Orders.Events;
global using ECommerce_Clean_Arch.Domain.Orders.ValueObjects;
global using ECommerce_Clean_Arch.Domain.Products;
global using ECommerce_Clean_Arch.Domain.Products.ValueObjects;
global using ECommerce_Clean_Arch.Domain.Users;
global using ECommerce_Clean_Arch.Domain.Users.Events;
global using ECommerce_Clean_Arch.Domain.UserSessions;
global using ECommerce_Clean_Arch.Domain.UserSessions.Enums;
global using ECommerce_Clean_Arch.Domain.UserSessions.Events;

global using FluentValidation;

global using MediatR;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;

global using SharedKernel.Errors;
global using SharedKernel.Models;
global using SharedKernel.Results;