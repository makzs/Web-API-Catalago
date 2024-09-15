﻿using APICatalago.Controllers;
using APICatalago.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalagoxUnitTest.UnitTests
{
    public class GetProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;

        public GetProdutoUnitTests(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task GetProdutoById_OkResult()
        {
            //Arrange
            var prodId = 3;

            //Act
            var data = await _controller.Get(prodId);

            //Assert (xunit)
            //var okResult = Assert.IsType<OkObjectResult>(data.Result);
            //Assert.Equal(200, okResult.StatusCode);

            // Assert (fluentassertion) -> verifica se o resultado é do tipo OkObjectResult
            data.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
        }
        [Fact]
        public async Task GetProdutoById_Return_NotFound()
        {
            //Arrange
            var prodId = 999;

            //Act
            var data = await _controller.Get(prodId);

            // Assert
            data.Result.Should().BeOfType<NotFoundResult>().Which.StatusCode.Should().Be(404);
        }
        [Fact]
        public async Task GetProdutos_Return_ListOfProdutoDTO()
        {

            //Act
            var data = await _controller.Get();

            // Assert
            data.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeAssignableTo<IEnumerable<ProdutoDTO>>()
                .And.NotBeNull();
        }
    }
}
