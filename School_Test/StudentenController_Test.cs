using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using School_Models;
using School_Models.Controllers;
using School_Models.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School_Test
{
    [TestClass]
    public class StudentenController_Test
    {
        private Task<List<Student>> studentsList = Task.FromResult(new List<Student>());

        [TestInitialize]
        public async Task GetFakeStudents()
        {
            for (int i = 0; i < 10; i++)
            {
                studentsList.Result.Add(new Student
                {
                    Id = i,
                    Name = "testAsync student" + i.ToString(),
                    Birthday = new System.DateTime(2018, 04, 10 + 1),
                    EducationId = 1000 + i,
                    Education = new Education()
                    {
                        Id = 1000 + i,
                        Name = "education" + i.ToString()
                    }
                });
            }
            await Task.FromResult(studentsList);
        }

        private async Task<Student> GetStudent(int id = 9)
        {
            var studentObj = studentsList.Result.Single(s => s.Id == id);
            return await Task.Run(() => studentObj);
        }
        [TestMethod]
        public async Task StudentController_Details_Returns_StudentObject()
        { //Arrange(ProductieRepo niet gebruiken) 
            var mockStudentRepo = new Mock<IStudentRepo>();
            mockStudentRepo.Setup(repo => repo.GetStudentAsync(9)).Returns(GetStudent(9));

            var controller = new StudentController(mockStudentRepo.Object, null, null);

            //Act (View hoeft niet te bestaan) 
            ViewResult result = await controller.Details(9) as ViewResult;

            //Assert (type en inhoud) 
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsInstanceOfType(result.Model, typeof(Student));
            Assert.AreEqual(9, ((Student)result.Model).Id);
            //Id niet verwarren met de Id van de testen. }

        }
    }
}
