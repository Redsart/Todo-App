using System;
using Xunit;
using Moq;
using TodoApp.Repositories.XmlRepository.Utils;
using System.Xml.Linq;
using TodoApp.Repositories.Models;
using Xml = TodoApp.Repositories.XmlRepository;

namespace TodoApp.Tests.Repositories.TodoRepositories
{
    public class WhenContextIsEmpty
    {
        protected Mock<IXmlContext> MockXmlContext;

        protected XElement Container;

        public WhenContextIsEmpty()
        {
            // Arrange
            MockXmlContext = new Mock<IXmlContext>();

            var containerName = "todos";
            Container = new XElement(containerName);

            MockXmlContext
                .Setup(ctx => ctx.GetContainer(containerName))
                .Returns(Container);
        }

        [Fact]
        public void GetAll_ReturnsEmpty()
        {
            // Arrange
            var repo = new Xml.TodoRepository(MockXmlContext.Object);

            // Act
            var all = repo.GetAll();

            // Assert
            Assert.Empty(all);
        }

        [Fact]
        public void GetById_ReturnsNull()
        {
            //Arange
            var repo = new Xml.TodoRepository(MockXmlContext.Object);

            //Act
            var todo = repo.GetById(Guid.Empty);

            //Assert
            Assert.Null(todo);
        }

        [Theory]
        [InlineData("", "Go to a picnic with friends", TodoStatus.Open, "2020-05-15T14:29:15.1823029Z", "2020-05-19T21:00:00.0000000Z")] // without title
        public void Insert_ThrowsArgumentException(string title, string description, TodoStatus status, string createdOn, string dueDate)
        {
            var repo = new Xml.TodoRepository(MockXmlContext.Object);
            var model = new TodoModel
            {
                Title = title,
                Description = description,
                Status = status,
                CreatedOn = DateTime.Parse(createdOn),
                DueDate = DateTime.Parse(dueDate)
            };

            var ex = Assert.Throws<ArgumentException>(() => repo.Insert(model));

            Assert.Equal("Empty todo!", ex.Message);
        }

        [Theory]
        [InlineData("", "Go to a picnic with friends", TodoStatus.Open, "", "2020-05-19T21:00:00.0000000Z")] // without createdOn
        [InlineData("", "Go to a picnic with friends", TodoStatus.Open, "2020-05-15T14:29:15.1823029Z", "")] // without dueDate
        public void GivenTodo_Insert_ThrowsFormatException(string title, string description, TodoStatus status, string createdOn, string dueDate)
        {
            var repo = new Xml.TodoRepository(MockXmlContext.Object);
            var model = new TodoModel
            {
                Title = title,
                Description = description,
                Status = status,
                CreatedOn = DateTime.Parse(createdOn),
                DueDate = DateTime.Parse(dueDate)
            };

            var ex = Assert.Throws<FormatException>(() => repo.Insert(model));

            Assert.Equal("Empty todo, wrong time!", ex.Message);
        }
    }
}
