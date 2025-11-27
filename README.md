
# Boutique Diayma — README

> Auteur : Dialika Thiaw
> Projet : TP — Prise en main .NET / VSCode / Rider / GitHub

---

## 1. Objectif du dépôt

Ce dépôt contient une petite application e-commerce ASP.NET Core fournie pour se familiariser avec un EDI (VSCode / Rider), exécuter et déboguer du code, gérer le code avec Git/GitHub et publier une application Windows autonome.

---

## 2. Contenu de la solution (projets)

La solution fournie contient **un seul projet** : `P2FixAnAppDotNetCode/Diayma.csproj` (application ASP.NET Core MVC).

Le projet contient les dossiers suivants (exemple) :

- `Controllers/` — Actions et logique de contrôleurs.
- `Models/` — Entités métiers (Product, Cart, Order, etc.).
- `Views/` — Razor views et composants.
- `wwwroot/` — ressources statiques (css, js, images).
- `Repositories/` — classes d'accès aux données (implémentation en mémoire pour cet exercice).
- `Resources/` — fichiers `.resx` pour la localisation (i18n).

Remarque : contrairement à certaines solutions multi-projets, ce dépôt est mono-projet (aucun dossier `Tests` ou `Domain` séparé).

---

## 3. Version SDK .NET utilisée

Le projet cible :

```xml
<TargetFramework>netcoreapp2.0</TargetFramework>
```

=> **.NET Core 2.0** (obsolète).
Le SDK installé sur la machine peut être plus récent (ex : .NET 10 SDK) ; cela provoque des avertissements sur le support et peut nécessiter quelques ajustements lors de la publication.

---

## 4. Installation du SDK (rappel)

* Si nécessaire, installe le SDK ciblé ou une version compatible :
   * Le projet cible `netcoreapp2.0` (obsolète). Pour le développement local, il est conseillé d'installer une version récente du SDK (par ex. .NET 6 ou .NET 10), mais migrer le projet peut demander des ajustements.
* Vérifier installé :

```bash
dotnet --list-sdks
dotnet --list-runtimes
```

---

## 5. Cloner le dépôt (instructions)

Pour cloner le dépôt (exemple) :

```bash
git clone <https://github.com/devehod/BoutiqueDiayma2025.git>
cd "BoutiqueDiayma2025"
cd P2FixAnAppDotNetCode
```
## 6. Exécution locale (développement)

Restaurer et exécuter :

```bash
cd P2FixAnAppDotNetCode
dotnet restore
dotnet run
```

Ou depuis VSCode : F5 / Start Debugging (configuration `.vscode/launch.json` ajoutée).

Si une fenêtre d’avertissement s’affiche (certificat/TLS/SDK) → fermer (OK) comme demandé.

L’application démarre normalement sur `http://localhost:62929` ou bien `http://localhost:5000 ` selon les machines 

---

## 7. Bugs trouvés (2 bugs signalés) — reproduction et correctifs

### Bug #1 — Calcul total du panier incorrect

**Fichier** : `P2FixAnAppDotNetCode/Models/Cart.cs`
**Problème** : la méthode `GetTotalValue()` additionne seulement les prix unitaires sans tenir compte de la quantité.
**Symptôme** : ajouter 2 unités d’un produit prix 10 affiche total = 10 (au lieu de 20).

**Exemple de code fautif (existant)** :

```csharp
// Mauvais : additionne seulement x.Product.Price
public decimal GetTotalValue()
{
    return GetCartLineList().Sum(x => x.Product.Price);
}
```

**Correction recommandée** :

```csharp
// Correct : tenir compte de la quantité
public decimal GetTotalValue()
{
    return GetCartLineList().Sum(x => x.Product.Price * x.Quantity);
}
```

> Après correction : test ajouter 1 puis 2 unités et vérifier affichage du total.

---

### Bug #2 — Sélecteur de langue (localisation) — diagnostic et résolution

**Symptôme** : le changement de culture/langue via le composant `LanguageSelector` ne traduit pas l’interface ou ne change rien.

**Pour le Vérifier et/ou le  Résoudre** :

- Utilisation  des fichiers `.resx`  (par ex. `Resources/Views/Product/Index.fr.resx`) et contiennent les clés traduites.
- Dans `Startup.ConfigureServices`, vérifions que `AddLocalization` et `RequestLocalizationOptions` contiennent toutes les cultures requises (ajouter `new CultureInfo("wo")` si on veut Wolof).
- Vérifions que des entrées `<Content Remove=...>` dans le `.csproj` ne suppriment pas les ressources ou views lors de la publication. Par exemple, `Diayma.csproj` enlève explicitement `Models/SessionCart.cs` et d'autres fichiers (`<Compile Remove="Models\SessionCart.cs" />`), ce qui doit être corrigé si on veut activer un `SessionCart`.
- Vérifions aussi l'implémentation du `LanguageController` et du `LanguageSelector` (cookie, Redirect) pour s'assurer que le `RequestCulture` est correctement mis en place.

Si tu veux ajouter Wolof (`wo`) : ajouter les fichiers `*.wo.resx` dans `Resources/Views/...` et insérer `new CultureInfo("wo")` à l'endroit des cultures supportées dans `Startup`.

---

## 8. Breakpoints demandés (recommandation)

Place des points d’arrêt aux emplacements suivants :

- `CartSummaryViewComponent` → constructeur et/ou méthode `Invoke()`.
- `ProductController.Index()` → point d’entrée de la page Produits.
- `OrderController.Index()` (POST) → pour suivre la validation et la sauvegarde de la commande.
- `CartController.Index()` et `CartController.AddToCart()` → pour suivre le flux d'ajout et suppression d'articles.
- `Startup.ConfigureServices()` et `Startup.Configure()` → pour vérifier l'enregistrement des services, la localisation et la session.

Pendant le debug, navigue vers la page d’accueil et observe l'ordre d'exécution et les variables locales.

---

## 9. Flux d’exécution avant affichage de la page Produits

> Mode de debug utilisé : **mix** de "Pas à pas principal", "Pas à pas détaillé" et "Pas à pas sortant" selon le niveau.

**Namespaces visités**

* `Microsoft.AspNetCore`
* `Microsoft.Extensions.DependencyInjection`
* `Microsoft.Extensions.Hosting`
* `P2FixAnAppDotNetCode`
* `P2FixAnAppDotNetCode.Controllers`
* `P2FixAnAppDotNetCode.Services`
* `P2FixAnAppDotNetCode.Repositories`
* `P2FixAnAppDotNetCode.Models`

**Classes visitées**
Program
Startup
ProductController
ProductService
ProductRepository
Product (model)

**Méthodes visitées**
Program.Main()
Startup.Startup()
Startup.ConfigureServices()
Startup.Configure()
ProductController.Index()
ProductService.GetAllProducts()
ProductRepository.GetAllProducts()

**Principales classes et méthodes (ordre typique)**

1. `Program.Main()` — **Pas à pas principal**

   * Configure l’hôte Web.

2. `Startup` (constructor) — **Pas à pas principal / sortant**

   * `Startup.ConfigureServices(IServiceCollection services)` — enregistrement des services (DI).
   * `Startup.Configure(IApplicationBuilder app, IHostingEnvironment env)` — pipeline (localization, static files, session, MVC routes).

3. `ProductController.Index()` — **Pas à pas détaillé**

   * Point d’entrée pour la page produits ; appelle le service produit.

4. `ProductService.GetAllProducts()` — **Pas à pas détaillé**

   * Logique métier / transformation.

5. `ProductRepository.GetAllProducts()` — **Pas à pas détaillé**

   * Accès aux données (en dur, fichier ou EF Core).

6. Retour à `ProductController` → `return View("Index", model)` — **Pas à pas sortant**

   * Moteur Razor cherche la vue `Index.cshtml` (chemins testés : `/Views/Product/Index*.cshtml`, `/Views/Shared/Index*.cshtml`).

7. Vue Razor `Index.cshtml` s’exécute et rend HTML.


---

## 10. Problèmes fréquents à la publication & solution

### Erreur fréquente : `The view 'Index' was not found`

* Cause : fichiers `.cshtml` (Views) manquent dans le dossier `publish`.
* Vérifie que les Views sont copiées lors de la publication.

### Inclusion des vues dans `.csproj`

Deux approches :

**A — Si le SDK inclut les items Content par défaut (SDK récent)**

* Ne pas ajouter explicitement `<Content Include="Views\**\*.cshtml" ... />` si cela crée des duplications.
* Supprimer les `Remove` inutiles si tu les as ajoutés.

**B — Si tu veux inclure explicitement (éviter duplication)**
Ajoute en tête du `.csproj` :

```xml
<PropertyGroup>
  <EnableDefaultContentItems>false</EnableDefaultContentItems>
</PropertyGroup>

<ItemGroup>
  <Content Include="Views\**\*.cshtml" CopyToOutputDirectory="PreserveNewest" />
</ItemGroup>
```

> Cette option évite les erreurs `NETSDK1022` (éléments Content dupliqués).

---

## 11. Publication en exécutable Windows (self-contained)

Commande recommandée :

```bash
dotnet publish -c Release -r win-x64 --self-contained true
```

* Dossier de sortie attendu (exemple) :

```
bin\Release\netcoreapp2.0\win-x64\publish\
```

* Le dossier `publish` doit contenir `Diayma.exe`, les DLL, `*.deps.json`, `*.runtimeconfig.json` et le dossier `Views\...` avec les `.cshtml`.
* Pour partager : zipper tout le contenu du dossier `publish` (ne pas partager seulement l’exe).

---

## 12. Où se trouve l’exécutable ?

Après `dotnet publish` :

```
C:\Users\HP\Desktop\DIC3\C# Technologies .Net\BoutiqueDiayma2025\P2FixAnAppDotNetCode\bin\Release\netcoreapp2.0\win-x64\publish\Diayma.exe
```

---

## 13. Partage de l’exécutable
Google drive 
`https://drive.google.com/file/d/1wsNi08wD4TH5aCxNBIMnGARWuOoGgTTy/view?usp=sharing`

## 14. Optionnel — Ajouter Wolof (localisation)

* Ajouter fichiers de ressources : `Resources/Views.Shared.fr.resx`, `Resources/Views.Shared.wo.resx`, etc.
* Configurer `RequestLocalizationOptions` dans `Startup.Configure`.
* Ajouter sélecteur de langue (`LanguageSelector`) pour changer la culture.


## 19. Ressources utiles

* .NET Core support : [https://aka.ms/dotnet-core-support](https://aka.ms/dotnet-core-support)

---


## Checklist (pour la remise / correction)

1. Project identification : `P2FixAnAppDotNetCode/Diayma.csproj`.
2. SDK : préciser `netcoreapp2.0` et recommander migration vers .NET 6/10.
3. Exécution : `dotnet run` (vérifier que les Views/Resources ne sont pas supprimées par le `.csproj`).
4. Debug : capture screenshot montrant breakpoints et valeurs locales.
5. Bugs corrigés : inclure au moins `fix(cart): GetTotalValue` et solution DI pour le `ICart` (session vs singleton).
6. Ajout d'une langue : ajouter `wo` et resx si demandé (optional).
7. Packaging : `dotnet publish -c Release -r win-x64 --self-contained true` et vérifier la présence des Views et Resources.
8. Confirmation : 3 commits significatifs, message clair et lien GitHub pour livraison.
