# Plugin de Metadados da Crunchyroll para Jellyfin

<p align="center">
  <img src="https://raw.githubusercontent.com/jellyfin/jellyfin-ux/master/branding/SVG/icon-transparent.svg" alt="Jellyfin Logo" width="100">
</p>

Um plugin de metadados para o Jellyfin que busca informações de animes diretamente da Crunchyroll, com mapeamento inteligente de temporadas e episódios para corresponder à forma como a maioria dos usuários organiza suas bibliotecas.

🔗 **Read this in English: [README.md](README.md)**

---

## ✨ Recursos

- **Metadados de Séries**: Título, sinopse, ano de lançamento, gêneros e classificação indicativa
- **Metadados de Temporadas**: Títulos e descrições das temporadas
- **Metadados de Episódios**: Título, sinopse, duração e data de exibição
- **Imagens**: Posters, backdrops e thumbnails de episódios
- **Suporte a Múltiplos Idiomas**: Inglês, Português (Brasil), Japonês e outros

---

## 🔥 FlareSolverr Obrigatório

> **⚠️ Desde a versão 2.0.0, o FlareSolverr é obrigatório para o funcionamento deste plugin.**

A Crunchyroll agora é protegida pelo Cloudflare, que bloqueia todas as requisições diretas à API. Este plugin utiliza o **Chrome DevTools Protocol (CDP)** dentro do navegador do FlareSolverr para contornar o Cloudflare e buscar os metadados.

Sem o FlareSolverr em execução, o plugin **não conseguirá obter nenhum metadado**.

📖 **[Guia de Instalação e Configuração do FlareSolverr](FLARESOLVERR.md)**

**Início rápido:**

```bash
docker run -d --name flaresolverr -p 8191:8191 ghcr.io/flaresolverr/flaresolverr:latest
```

Em seguida, configure o plugin com a URL do FlareSolverr (`http://localhost:8191`) e o nome do container Docker (`flaresolverr`) nas configurações do plugin.

---

## 🎯 Problemas que Este Plugin Resolve

### Temporadas Separadas (comportamento estilo AniDB)

Alguns provedores de metadados tratam cada temporada como uma série separada. Este plugin evita isso ao:

- Mapear automaticamente as temporadas do Jellyfin para as temporadas da Crunchyroll
- Manter todas as temporadas agrupadas sob uma única série

---

### Numeração Contínua de Episódios

A Crunchyroll às vezes utiliza numeração contínua de episódios entre temporadas.

Exemplo:

- **Jujutsu Kaisen**: a Temporada 2 começa no episódio 25 na Crunchyroll
- **Biblioteca típica no Jellyfin**: a Temporada 2 começa no episódio 1

Este plugin utiliza **cálculo automático de offset de episódios**, garantindo:

- `S02E01` no Jellyfin → Episódio 25 na Crunchyroll ✅
- `S02E02` no Jellyfin → Episódio 26 na Crunchyroll ✅

---

## 📦 Instalação

### Método 1: Repositório de Plugins (Recomendado)

1. Abra o Dashboard do Jellyfin
2. Vá em `Dashboard > Plugins > Repositories`
3. Clique em `+` e adicione a seguinte URL de manifesto:

```
https://raw.githubusercontent.com/einznoah/crunchyroll-jellyfin/main/manifest.json
```

4. Salve e vá para `Dashboard > Plugins > Catalog`
5. Procure por **Crunchyroll Metadata** e clique em **Install**
6. Reinicie o Jellyfin

```bash
# Linux (systemd)
sudo systemctl restart jellyfin

# Docker
docker restart jellyfin
```

---

### Método 2: Instalação Manual

1. Baixe `Jellyfin.Plugin.Crunchyroll.zip` na página de Releases
2. Extraia os arquivos para o diretório de plugins apropriado:

| Sistema | Caminho                                               |
| ------- | ----------------------------------------------------- |
| Linux   | `/var/lib/jellyfin/plugins/Crunchyroll/`              |
| Windows | `C:\ProgramData\Jellyfin\Server\plugins\Crunchyroll\` |
| macOS   | `~/.local/share/jellyfin/plugins/Crunchyroll/`        |
| Docker  | `/config/plugins/Crunchyroll/`                        |

> Crie a pasta `Crunchyroll` caso ela não exista.

3. Reinicie o Jellyfin

---

### Método 3: Compilando a Partir do Código Fonte

```bash
git clone https://github.com/einznoah/crunchyroll-jellyfin.git
cd crunchyroll-jellyfin
dotnet build -c Release
```

A DLL compilada estará localizada em:

```
Jellyfin.Plugin.Crunchyroll/bin/Release/net8.0/Jellyfin.Plugin.Crunchyroll.dll
```

Copie-a para o diretório de plugins do Jellyfin e reinicie o servidor.

---

## ⚙️ Configuração

Configure o plugin em:

```
Dashboard > Plugins > Crunchyroll Metadata
```

### Idioma

- **Idioma Preferido**: Idioma principal dos metadados
- **Idioma de Fallback**: Utilizado quando o idioma preferido não está disponível

### FlareSolverr (Obrigatório)

- **URL do FlareSolverr**: URL da instância do FlareSolverr (padrão: `http://localhost:8191`)
- **Nome do Container Docker**: Nome do container Docker do FlareSolverr (padrão: `flaresolverr`)
- **URL do Chrome CDP**: _(Avançado)_ Sobrescreve a URL do Chrome DevTools Protocol. Deixe vazio para detecção automática.

> 📖 Veja **[FLARESOLVERR.md](FLARESOLVERR.md)** para instruções detalhadas de configuração.

### Mapeamento de Temporadas e Episódios

- **Habilitar Mapeamento de Temporadas**: Mapeia temporadas do Jellyfin para a Crunchyroll
- **Habilitar Mapeamento de Offset de Episódios**: Trata automaticamente a numeração contínua

### Cache

- **Expiração do Cache**: Duração do cache de metadados em horas (padrão: 24h)

---

## 🔧 Uso

### Configuração da Biblioteca de Animes

1. Crie ou edite uma biblioteca do tipo Séries de TV
2. Defina o tipo de conteúdo como **Shows**
3. Ative **Crunchyroll** em:
   - Provedores de metadados de Séries
   - Provedores de metadados de Temporadas
   - Provedores de metadados de Episódios
4. Ative **Crunchyroll** em Provedores de Imagens
5. Ajuste a prioridade conforme desejar

---

### Organização Recomendada de Arquivos

```text
Animes/
├── Jujutsu Kaisen/
│   ├── Season 1/
│   │   ├── Jujutsu Kaisen - S01E01 - Ryomen Sukuna.mkv
│   │   └── ...
│   └── Season 2/
│       ├── Jujutsu Kaisen - S02E01 - Hidden Inventory.mkv
│       └── ...
```

---

### Identificação Manual

Caso o plugin não identifique automaticamente uma série:

1. Abra a série no Jellyfin
2. Clique em **Editar Metadados**
3. Selecione **Identificar**
4. Busque pelo título na Crunchyroll
5. Escolha o resultado correto e atualize os metadados

---

## 🐛 Solução de Problemas

### Série não encontrada

- Verifique se o nome corresponde ao usado pela Crunchyroll
- Utilize a identificação manual
- Confirme se o anime está disponível na Crunchyroll

### Idioma incorreto

- Verifique as configurações de idioma do plugin
- Nem todos os títulos possuem localização completa

### Episódios incorretos

- Confirme se o mapeamento de offset está habilitado
- Verifique se cada temporada inicia no episódio 1 localmente

### Logs de Debug

Ative logs detalhados em `Dashboard > Logs` e procure por `Crunchyroll`.

---

## 🔄 Atualizações

Quando instalado via repositório, o Jellyfin notificará automaticamente sobre novas versões disponíveis.

---

## 🤝 Contribuindo

Contribuições são bem-vindas!

1. Faça um fork do repositório
2. Crie uma branch para sua feature
3. Faça commit das alterações
4. Envie para seu fork
5. Abra um Pull Request

---

## 📄 Licença

Este projeto está licenciado sob a Licença MIT. Consulte `LICENSE.md` para mais detalhes.

---

## ⚠️ Aviso Legal

Este plugin não é afiliado, endossado ou patrocinado pela Crunchyroll ou pela Sony.

Crunchyroll é uma marca registrada da Sony Group Corporation.

Este plugin utiliza apenas metadados disponíveis publicamente e não fornece acesso a conteúdo premium ou protegido por direitos autorais.

---

## 🙏 Agradecimentos

- Projeto Jellyfin e comunidade de desenvolvedores de plugins
- Projetos de documentação não-oficial da API da Crunchyroll

<p align="center">
  Feito com ❤️ para a comunidade do Jellyfin
</p>
