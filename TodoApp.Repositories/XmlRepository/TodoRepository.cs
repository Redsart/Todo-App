﻿using System;
using System.Xml.Linq;
using TodoApp.Repositories.Models;
using TodoApp.Repositories.Interfaces;
using TodoApp.Repositories.XmlRepository.Utils;

namespace TodoApp.Repositories.XmlRepository
{
    public class TodoRepository : RepositoryBase<TodoModel, Guid>, ITodoRepository
    {
        public TodoRepository(IXmlContext context) : base(context, "todos")
        {

        }

        protected override string IdName => "Id";

        protected override TodoModel ElementToEntity(XElement element)
        {
            if (element == null)
            {
                return null;
            }

            var entity = new TodoModel();
            entity.Title = XmlParser.GetString(element, "Title");
            entity.Description = XmlParser.GetString(element, "Description");
            entity.Status = XmlParser.GetEnum<TodoStatus>(element, "Status");
            entity.CreatedOn = XmlParser.GetDateTime(element, "CreatedOn");
            entity.DueDate = XmlParser.GetDateTime(element, "DueDate");
            entity.Id = XmlParser.GetGuid(element, IdName);

            if (string.IsNullOrEmpty(element.Element("Title").Value) ||
                string.IsNullOrEmpty(element.Element("Status").Value))
            {
                throw new ArgumentException("Empty todo!");
            }

            if (string.IsNullOrEmpty(element.Element("CreatedOn").Value) ||
            string.IsNullOrEmpty(element.Element("DueDate").Value))
            {
                throw new FormatException("Empty todo, wrong time!");
            }

            return entity;
        }

        protected override XElement EntityToElement(TodoModel entity)
        {
            if (entity == null)
            {
                return null;
            }

            var element = new XElement("todo");
            XmlParser.SetString(element, "Title", entity.Title);
            XmlParser.SetString(element, "Description", entity.Description);
            XmlParser.SetEnum<TodoStatus>(element, "Status", entity.Status);
            XmlParser.SetDateTime(element, "CreatedOn", entity.CreatedOn);
            XmlParser.SetDateTime(element, "DueDate", entity.DueDate);
            XmlParser.SetGuid(element, IdName, entity.Id);

            if (string.IsNullOrEmpty(element.Element("Title").Value) || 
                string.IsNullOrEmpty(element.Element("Status").Value))
            {
                throw new ArgumentException("Empty todo!");
            }

            if (string.IsNullOrEmpty(element.Element("CreatedOn").Value) ||
            string.IsNullOrEmpty(element.Element("DueDate").Value))
            {
                throw new FormatException("Empty todo, wrong time!");
            }

            return element;
        }
    }
}
