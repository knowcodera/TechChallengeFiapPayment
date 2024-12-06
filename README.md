
# Payment - Microsserviço

Este repositório contém o código-fonte do microsserviço **Payment**, responsável pela gestão e processamento de pagamentos no sistema da lanchonete.

---

## 🔧 **Descrição**
O microsserviço **Payment** gerencia:
- Registro de solicitações de pagamento.
- Comunicação com o processador de pagamentos.
- Atualização do status dos pedidos após a confirmação ou rejeição do pagamento.

---

## 🚀 **Tecnologias**
Este projeto utiliza as seguintes tecnologias:
- **Linguagem:** C# com .NET 8.0
- **Banco de Dados:** SQL Server
- **Mensageria:** RabbitMQ
- **Infraestrutura:** Azure Kubernetes Service (AKS), Azure Container Registry (ACR)
- **CI/CD:** GitHub Actions
- **Teste e Qualidade de Código:** SonarQube

---

## 🛠️ **Configuração**
### **Pré-requisitos**
1. **Infraestrutura**:
   - Azure AKS configurado.
   - Azure Container Registry configurado.
   - RabbitMQ rodando como serviço de mensageria.
   - Banco de dados SQL Server configurado.
2. **Ferramentas**:
   - Docker
   - Azure CLI
   - Terraform

---

## 🧪 **Testes**
Os testes do projeto foram implementados utilizando **xUnit** com cobertura mínima de 80%.


### **Evidência de Cobertura**
- **Screenshot da Cobertura de Testes**:

---

## 📦 **CI/CD**
O pipeline CI/CD está configurado no GitHub Actions:
- Realiza o build e testes automatizados.
- Publica a imagem Docker no Azure Container Registry.
- Faz o deploy no Azure Kubernetes Service (AKS).
- Faz o teste de cobertua via sonarcloud - SonarQube  de 70%.

### **Workflow Configurado**
Confira o workflow em:
```bash
.github/workflows/workflow.yml
```

---

## Licença
Este projeto está licenciado sob a licença MIT. Consulte o arquivo LICENSE para obter mais detalhes.
