# Model Context Protocol (MCP) Demos

[Model Context Protocol (MCP)](https://modelcontextprotocol.io/overview)

[MCP Servers](https://github.com/modelcontextprotocol/servers)

[Extend your agent with Model Context Protocol](https://learn.microsoft.com/en-us/microsoft-copilot-studio/agent-extend-action-mcp)

## Demos

Run the sample MCP server in src/hr-mcp-server:

```bash
dotnet run
```

Run MCP inspector:

```bash
npx @modelcontextprotocol/inspector node build/index.js
```

Create a new Custom Connector of type `"Import an OpenAPI file"` and import the OpenAPI file from `src/hr-mcp-server/hr-mcp.yaml`.
