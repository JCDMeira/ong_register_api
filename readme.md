# ong_register_api - api para registro de ongs

<p align="center">
  <image
  src="https://img.shields.io/github/languages/count/JCDMeira/ong_register_api"
  />
  <image
  src="https://img.shields.io/github/languages/top/JCDMeira/ong_register_api"
  />
  <image
  src="https://img.shields.io/github/last-commit/JCDMeira/ong_register_api"
  />
  <image
  src="https://img.shields.io/github/watchers/JCDMeira/ong_register_api?style=social"
  />
</p>

# 📋 Indíce

- [Proposta](#id01)
  - [Conclusões](#id01.01)
- [Requisitos](#id02)
  - [Requisitos funcionais](#id02.1)
  - [Requisitos não funcionais](#id02.2)
  - [Requisitos não obrigatórios](#id02.3)
- [Aprendizados](#id03)
- [Feito com](#id04)
- [Pré-requisitos](#id05)
- [Procedimentos de instalação](#id06)
- [Autor](#id07)

# 🚀 Proposta <a name="id01"></a>

Este é o projeto tem como objetivo central a criação de uma api de registro para ongs. Em sua essência permite resgistrar ongs conhecidas e mais informações quanto a propósito e como é possível ajudar.

# 🎯 Requisitos <a name="id02"></a>

## 🎯 Requisitos funcionais <a name="id02.1"></a>

Sua aplicação deve ter:

- Um end-point para cada um dos métodos get, put, post e delete para a ong
- Deve ser possível criar um registro de ong
- Deve ser possível editar um registro de ong
- Deve ser possível deletar um registro de ong
- Deve ser possível buscar a lista de ongs já existentes
- O get deve ser paginado
  - a paginação deve aceitar quantos itens retorna por página
- Um registro de ong deve ter
  - Nome
  - descrição
  - url imagem
  - propósito (ajuda cães, luta contra a fome infantil....)
  - Como é possível ajudar.

## 🎯 Requisitos não funcionais <a name="id02.2"></a>

É obrigatório a utilização de:

- .net
- mongoDB

## 🎯 Requisitos não obrigatórios <a name="id02.3"></a>

- buscar por nome de ong

# Aprendizados <a name="id03"></a>

Foi usado um pacote chamado `X.PagedList` para montar a paginação com parâmetros de page e count, sendo page a página que retorna e count a quantidade de itens por página.

A rota específica já reconheceu o parâmetro ao ser passado para a função, mas é preciso tratar, já quu pode ser que não venha. Ou seja, é algo opcional.

```c#
using Microsoft.AspNetCore.Mvc;
using OngResgisterApi.Models;
using OngResgisterApi.utils;
using RestaurantApi.Services;
using X.PagedList;

namespace OngResgisterApi.Controllers
{
    [Route("/api/ongs")]
    [ApiController]
    public class OngsController : Controller
    {
        private OngsService _ongsService;

        public OngsController(OngsService service) => _ongsService = service;

        [HttpGet]
        public async Task<IActionResult> Get(int? page, int? count) {
            int pageList = page ?? 1;
            int pageCount = count ?? 20;

            var result = await _ongsService.GetAsync();
            var pagedResult = result.ToPagedList(pageList, pageCount);

            return Ok(pagedResult);
         }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Ong>> Get(string id)
        {
            var ong = await _ongsService.GetAsync(id);
            if(ong == null) return NotFound();
            return Ok(ong);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Ong newOng)
        {
            var ong = await _ongsService.GetByNameAsync(newOng.Name);
            if(ong != null) return BadRequest();
            if (newOng.ImageUrl == null || newOng.ImageUrl == "")
                newOng.ImageUrl = Image.GetImageFallback();
            if (!Uri.IsWellFormedUriString(newOng.ImageUrl, UriKind.RelativeOrAbsolute)) return BadRequest();
            await _ongsService.CreateAsync(newOng);
            return CreatedAtAction(nameof(Get), new {id= newOng.Id}, newOng);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Ong updatedOng)
        {
            var ong = await _ongsService.GetAsync(id);
            if (ong == null) return NotFound();

            var existingOng = await _ongsService.GetByNameAsync(updatedOng.Name);
            if (existingOng != null && existingOng.Id != id) return BadRequest();

            if (updatedOng.ImageUrl == null || updatedOng.ImageUrl == "")
                updatedOng.ImageUrl = ong.ImageUrl;

            if (!Uri.IsWellFormedUriString(updatedOng.ImageUrl, UriKind.RelativeOrAbsolute)) return BadRequest();
            updatedOng.Id = ong.Id;
            await _ongsService.UpdateAsync(id, updatedOng);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var ong = await _ongsService.GetAsync(id);
            if(ong == null) return NotFound();
            await _ongsService.RemoveAsync(id);
            return NoContent();
        }
    }
}
```

## Considerações

Há mais formar de fazer paginação, nesse modelo se usa page size, que basicamente monta páginas e itens por página. Também há o modelo de page hash, que trabalha com ancoras after e before.

Será interessante testar o modelo de page hash em alguma outra aplicação.

Como uma melhoria desse projeto se buscará realizar a padronização do retorno de erro.

## Pontos de melhoria

Foi adotado uma classe utilitária que permite buscar por texto dentro de uma string.

```c#
using System.Text.RegularExpressions;

namespace restaurant_api.Utils
{
    public static class EntintyFilters
    {
        public static bool HasSearchString(string name, string? searchString)
        {
            if (searchString == null) return true;
            var regex = new Regex($"{searchString}", RegexOptions.IgnoreCase);
            return regex.IsMatch(name);
        }
    }
}

```

Então se mudou o getAll da aplicação para trabalhar com um retorno do tipo `Task<ActionResult<List<Ong>>>` e usando conjuntos de parâmetros opcionais do tipo `FromQuery`, para identificar que são providos das querys. E agora o retorno passa pelo `OK()` para respeitar o tipo `ActionResult`.

```c#

        [HttpGet]
        public async Task<ActionResult<List<Ong>>> Get(
            int? page,
            int? count,
            [FromQuery] string? name,
            [FromQuery] string? purpose,
            [FromQuery] string? search,
            [FromQuery] string? how_to_assist
            ) {
            int pageList = page ?? 1;
            int pageCount = count ?? 20;

            var result = await _ongsService.GetAsync();
            var pagedResult = result
                .Where(ong =>
                EntintyFilters.HasSearchString(ong.Name,name) &&
                EntintyFilters.HasSearchString(ong.Purpose, purpose) &&
                ong.HowToAssist.Any(s => EntintyFilters.HasSearchString(s, how_to_assist)) &&
                    (
                    EntintyFilters.HasSearchString(ong.Name, search) ||
                    EntintyFilters.HasSearchString(ong.Purpose, search) ||
                    ong.HowToAssist.Any(s => EntintyFilters.HasSearchString(s, search))
                    )
                )
                .ToPagedList(pageList, pageCount);

            return Ok(pagedResult);
         }
```

No filtro se usou apenas um where para não aumentar a complexidade n das execusões. Pra isso se usou um conjunto de funções que retornam boleanos para validar uma condição de filtro do where. Para filtros de campo específico com o && (e) e dentro da condição avançada se faz o || (ou) para que se busque a pertinencia para qualquer um dos campos.

```c#
  .Where(ong =>
                EntintyFilters.HasSearchString(ong.Name,name) &&
                EntintyFilters.HasSearchString(ong.Purpose, purpose) &&
                ong.HowToAssist.Any(s => EntintyFilters.HasSearchString(s, how_to_assist)) &&
                    (
                    EntintyFilters.HasSearchString(ong.Name, search) ||
                    EntintyFilters.HasSearchString(ong.Purpose, search) ||
                    ong.HowToAssist.Any(s => EntintyFilters.HasSearchString(s, search))
                    )
                )
```

nota: Validar melhor como funcionaria migrar essa estrutura para uma arquitetura centrada usando DDD.

- como ficaria a divisão das camadas?
- como eu mudaria a estrutura de pastas e faria as camadas se comunicarem?
- qual seria o papel do repositório nessa nova abordagem?
- quais papeis são carregados pela camada de service?
- controller só repassa a chamada ?
- quem no final devolve o status?
- como padronizo os retornos e tratamentos de erros?

# 🛠 Feito com <a name="id04"></a>

<br />

- C#
- .net 8
- visual studio
- mongoDB

<br />

# :sunglasses: Autor <a name="id07"></a>

<br />

- Linkedin - [jeanmeira](https://www.linkedin.com/in/jeanmeira/)
- Instagram - [@jean.meira10](https://www.instagram.com/jean.meira10/)
- GitHub - [JCDMeira](https://github.com/JCDMeira)
