using API_Developers.Model;
using API_Developers.Repository.Generic;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace API_Developers.Business.Implementations
{
    public class DeveloperBusinessImplementation : IDeveloperBusiness
    {
        private readonly IRepository<Developer> _repository;

        public DeveloperBusinessImplementation(IRepository<Developer> repository)
        {
            _repository = repository;
        }
        public List<Developer> FindAll()
        {
            _repository.DeleteAll();
            GetAllDevelopers().Wait();
            return _repository.FindAll();
        }

        public Developer FindById(long id)
        {
            _repository.Delete(id);
            GetDeveloperById(id).Wait();
            return _repository.FindById(id);
        }

        public async Task<Developer> Create(Developer developer)
        {
            var id = await SendDeveloper(developer);
            return _repository.FindById(id);
        }

        public Developer Update(Developer developer)
        {
            UpdateDeveloper(developer).Wait();
            return _repository.FindById(developer.Id);
        }
        public void Delete(long id)
        {
            DeleteDeveloper(id).Wait();
        }




        static readonly Uri _baseAddress = new Uri("https://63d99615b28a3148f675f67e.mockapi.io");
        public async Task GetAllDevelopers()
        {
            using (var client = new HttpClient { BaseAddress = _baseAddress })
            {
                var response = await client.GetAsync("dev");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    var developers = await response.Content.ReadAsAsync<Developer[]>();
                    SaveDevelopers(developers);
                }
            }
        }
        public async Task GetDeveloperById(long id)
        {
            using (var client = new HttpClient { BaseAddress = _baseAddress })
            {
                var response = await client.GetAsync($"dev/{id.ToString()}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    SaveDevelopersJson(content);
                }
            }
        }
        public async Task<long> SendDeveloper(Developer developer)
        {
            using (var client = new HttpClient { BaseAddress = _baseAddress })
            {
                string json = JsonConvert.SerializeObject(developer);
                var body = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("dev", body);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    Developer newdev = SaveDevelopersJson(content);
                    return newdev.Id;
                }
                else
                {
                    return 0;
                }
            }
        }
        public async Task UpdateDeveloper(Developer developer)
        {
            using (var client = new HttpClient { BaseAddress = _baseAddress })
            {
                string json = JsonConvert.SerializeObject(developer);
                var body = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.GetAsync($"dev/{developer.Id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    UpdateDeveloperJson(content);
                }
            }
        }
        public async Task DeleteDeveloper(long id)
        {
            using (var client = new HttpClient { BaseAddress = _baseAddress })
            {
                var response = await client.DeleteAsync($"dev/{id}");
                if (response.IsSuccessStatusCode)
                {
                    _repository.Delete(id);
                }
            }
        }


        private void SaveDevelopers(Developer[] developers)
        {
            foreach (var dev in developers)
            {
                dev.Email = ModifyEmail(dev.Email);
                _repository.Create(dev);
            }
                
        }

        private string? ModifyEmail(string? email)
        {
            if (!EmailValid(email))
                return null;

            var arrEmail = email.Split('@');
            if (arrEmail.Length == 2)
            {
                // Verifica se o domínio do e-mail está fora do padrão.
                if (arrEmail[1] != "padrao.com.br")
                {
                    // Corrige o e-mail. Deixei o comentário no final para demonstração no console.
                    return arrEmail[0] + "@padrao.com.br (modificado, antes era '@" + arrEmail[1] + "')";
                }
                else
                {
                    // Email correto, retorna a mesma entrada.
                    return email;
                }
            }
            // Poderíamos aplicar algum tratamento de remover o e-mail (ou setar alguma flag de email inválido),
            // mas deixei para demonstração no console.
            return email += " (não é um email válido!)";
        }

        private static bool EmailValid(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
            email = email.Trim();
            Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");
            return rg.IsMatch(email);
        }

        private Developer SaveDevelopersJson(string content)
        {
            Developer dev = JsonConvert.DeserializeObject<Developer>(content);
            dev.Email = ModifyEmail(dev.Email);
            return _repository.Create(dev);
        }
        private void UpdateDeveloperJson(string content)
        {
            Developer dev = JsonConvert.DeserializeObject<Developer>(content);
            _repository.Update(dev);
        }
    }
}
