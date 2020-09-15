using GalaSoft.MvvmLight;
using SimpleReflector.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleReflector.Model
{
    class OpenedFile
    {
        private Assembly assembly;
        private Type[] types;
        private StringBuilder description;

        public OpenedFile(string filePath)
        {            
            assembly = Assembly.LoadFrom(filePath);
            types = assembly.GetTypes();   
        }

        public ObservableCollection<AssemblyTypeNode> GetAssemblyTypes()
        {
            var classesTree = new ObservableCollection<AssemblyTypeNode>();
            
            foreach (var type in types)
            {            
                var members = type.GetMembers();
                AssemblyTypeNode treeViewNode = new AssemblyTypeNode
                {
                    AssemblyType = type,
                    TypeMembers = new ObservableCollection<MemberInfo>()
                };

                foreach (var member in members)
                {
                    treeViewNode.TypeMembers.Add(member);
                }

                classesTree.Add(treeViewNode);
            }

            return classesTree;
        }

        public string GetMemberDescription(MemberInfo member)
        {
            description = new StringBuilder();           
            description.AppendLine($"Имя: {member.Name}");
            description.AppendLine($"Тип: {member.MemberType}");
            description.AppendLine($"Класс: {member.DeclaringType.Name}\n");

            switch (member.MemberType)
            {
                case MemberTypes.Constructor:
                    description.AppendLine($"Модификатор public: {((ConstructorInfo)member).IsPublic}");
                    description.AppendLine($"Модификатор private: {((ConstructorInfo)member).IsPrivate}");
                    description.AppendLine($"Модификатор static: {((ConstructorInfo)member).IsStatic}\n");
                    GetParametersDescription(((ConstructorInfo)member).GetParameters());
                    break;
                case MemberTypes.Event:
                    break;
                case MemberTypes.Field:
                    description.AppendLine($"Модификатор public: {((FieldInfo)member).IsPublic}");
                    description.AppendLine($"Модификатор private: {((FieldInfo)member).IsPrivate}");
                    description.AppendLine($"Модификатор static: {((FieldInfo)member).IsStatic}\n");
                    description.AppendLine($"Тип поля: {((FieldInfo)member).FieldType.Name}\n");
                    break;
                case MemberTypes.Method:
                    description.AppendLine($"Модификатор public: {((MethodInfo)member).IsPublic}");
                    description.AppendLine($"Модификатор private: {((MethodInfo)member).IsPrivate}");
                    description.AppendLine($"Модификатор static: {((MethodInfo)member).IsStatic}\n");
                    description.AppendLine($"Тип возвращаемого значения: {((MethodInfo)member).ReturnType.Name}\n");
                    GetParametersDescription(((MethodInfo)member).GetParameters());
                    break;
                case MemberTypes.Property:
                    description.AppendLine($"Тип свойства: {((PropertyInfo)member).PropertyType.Name}\n");
                    break;
                case MemberTypes.TypeInfo:
                    break;
                case MemberTypes.Custom:
                    break;
                case MemberTypes.NestedType:
                    break;
                case MemberTypes.All:
                    break;
                default:
                    break;
            }

            return description.ToString();
        }

        private void GetParametersDescription(ParameterInfo[] parameters)
        {
            description.AppendLine($"Количество параметров: {parameters.Length}\n");

            foreach (var parameter in parameters)
            {
                description.AppendLine($"Имя: {parameter.Name}");
                description.AppendLine($"Позиция: {parameter.Position}");
                description.AppendLine($"Тип: {parameter.ParameterType.Name}\n");
            }
        }
    }

    public class AssemblyTypeNode
    {
        public Type AssemblyType { get; set; }
        public ObservableCollection<MemberInfo> TypeMembers { get; set; }

    }   
}
