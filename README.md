# Pure.DI 2.5.8 の `overriddenBoolean` / CS0103 再現

.NET 10 と Pure.DI だけに依存する単独プロジェクトです。既定の Pure.DI 2.5.8 でビルドすると、生成された `Composition.g.cs` で `overriddenBoolean` がスコープ外から参照され、CS0103 が発生します。

## 実行方法

このディレクトリで次を実行します。

```powershell
dotnet build Repro.csproj -p:PureDIVersion=2.5.8
dotnet build Repro.csproj -p:PureDIVersion=2.5.6
```

実測結果（両方とも終了コード 1）：2.5.8 では `Composition.g.cs(117,35)` の `overriddenBoolean` に対する **CS0103**、2.5.6 では `Composition.cs(19,33)` の未登録 `bool` に対する **DIE000** が発生します。`FileSystem` を直接ルートにしているため、同じソースで「2.5.6 成功、2.5.8 失敗」を示す再現ではありません。

`Func<bool, FileSystem>`、`Lazy<Dialogs>`、singleton 登録、複数のルートを含む小さな依存グラフです。生成コードは `obj/generated` 以下で確認できます。このプロジェクトが示すのは 2.5.8 の CS0103 という生成結果であり、Filedini のバージョン更新が失敗する原因まで特定するものではありません。

