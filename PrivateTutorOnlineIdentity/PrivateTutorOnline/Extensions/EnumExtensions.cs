using PrivateTutorOnline.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace PrivateTutorOnline.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
        public static string GetDegreeName(this AcademicDegree degreeEnum)
        {
            switch (degreeEnum)
            {
                case AcademicDegree.Student:
                    {
                        return "Sinh viên";
                        break;
                    }
                case AcademicDegree.Teacher:
                    {
                        return "Giáo viên";
                        break;
                    }
                case AcademicDegree.Lecturer:
                    {
                        return "Giảng viên";
                        break;
                    }
                case AcademicDegree.Bachelor:
                    {
                        return "Cử nhân";
                        break;
                    }
                case AcademicDegree.Engineer:
                    {
                        return "Kĩ sư";
                        break;
                    }
                case AcademicDegree.Master:
                    {
                        return "Thạc sĩ";
                        break;
                    }
                case AcademicDegree.Doctor:
                    {
                        return "Tiến sĩ";
                        break;
                    }
                default:
                    {
                        return "Khác";
                        break;
                    }
            }
        }
        public static string GetGenderName(this Gender genderEnum)
        {
            switch (genderEnum)
            {
                case Gender.Male:
                    {
                        return "Nam";
                        break;
                    }
                default:
                    {
                        return "Nữ";
                        break;
                    }
            }
        }
    }
}