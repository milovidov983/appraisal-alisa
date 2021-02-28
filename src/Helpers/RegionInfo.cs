using System;
using System.Collections.Generic;
using System.Text;

namespace AliceAppraisal.Helpers {

	public class RegionItem {
		/// <summary>
		/// Код региона
		/// </summary>
		public int Code { get; set; }
		/// <summary>
		/// Наименование региона
		/// </summary>
		public string RegionName { get; set; }
		/// <summary>
		/// Наименование столицы региона
		/// </summary>
		public string CapitalName { get; set; }
		/// <summary>
		/// Код ФИАС региона
		/// </summary>
		public Guid CapitalId { get; set; }
		/// <summary>
		/// Код ФИАС столицы региона
		/// </summary>
		public Guid RegionId { get; set; }
		/// <summary>
		/// Город в котором находится офис продаж, обслуживающий данный регион.
		/// </summary>
		public int? ParentCityCode { get; set; }

		public static int[] FavoriteCityIndex { get; } = new[] { 76, 77, 65, 53, 15, 51, 73, 62, 54, 60, 1, 23, 35, 58, 33 };

		public static RegionItem[] All { get; set; } = new RegionItem[] {
			new RegionItem { ParentCityCode = 23, Code = 1, RegionName = "Адыгея", CapitalName = "Майкоп", RegionId = new Guid("d8327a56-80de-4df2-815c-4f6ab1224c50"), CapitalId = new Guid("8cfbe842-e803-49ca-9347-1ef90481dd98") },
			new RegionItem { ParentCityCode = 77, Code = 2, RegionName = "Башкортостан", CapitalName = "Уфа", RegionId = new Guid("6f2cbfd8-692a-4ee4-9b16-067210bde3fc"), CapitalId = new Guid("7339e834-2cb4-4734-a4c7-1fca2c66e562") },
			new RegionItem { ParentCityCode = null, Code = 3, RegionName = "Бурятия", CapitalName = "Улан-Удэ", RegionId = new Guid("a84ebed3-153d-4ba9-8532-8bdf879e1f5a"), CapitalId = new Guid("9fdcc25f-a3d0-4f28-8b61-40648d099065") },
			new RegionItem { ParentCityCode = null, Code = 4, RegionName = "Алтай", CapitalName = "Горно-Алтайск", RegionId = new Guid("5c48611f-5de6-4771-9695-7e36a4e7529d"), CapitalId = new Guid("0839d751-b940-4d3d-afb6-5df03fdd7791") },
			new RegionItem { ParentCityCode = null, Code = 5, RegionName = "Дагестан", CapitalName = "Махачкала", RegionId = new Guid("0bb7fa19-736d-49cf-ad0e-9774c4dae09b"), CapitalId = new Guid("727cdf1e-1b70-4e07-8995-9bf7ca9abefb") },
			new RegionItem { ParentCityCode = null, Code = 6, RegionName = "Ингушетия", CapitalName = "Магас", RegionId = new Guid("b2d8cd20-cabc-4deb-afad-f3c4b4d55821"), CapitalId = new Guid("c801edb4-aba2-4e1d-9ab2-69fcbf0aeb9c") },
			new RegionItem { ParentCityCode = 23, Code = 7, RegionName = "Кабардино-Балкарская", CapitalName = "Нальчик", RegionId = new Guid("1781f74e-be4a-4697-9c6b-493057c94818"), CapitalId = new Guid("913a82e3-b671-43d5-97b4-8a08b8ee2d28") },
			new RegionItem { ParentCityCode = 61, Code = 8, RegionName = "Калмыкия", CapitalName = "Элиста", RegionId = new Guid("491cde9d-9d76-4591-ab46-ea93c079e686"), CapitalId = new Guid("d5bd18b9-22c1-48e2-9b4a-3b7a4c89a3cb") },
			new RegionItem { ParentCityCode = 23, Code = 9, RegionName = "Карачаево-Черкесская", CapitalName = "Черкесск", RegionId = new Guid("61b95807-388a-4cb1-9bee-889f7cf811c8"), CapitalId = new Guid("2a4a7c93-f3f8-4042-8cbf-e04ab64f5e08") },
			new RegionItem { ParentCityCode = 77, Code = 10, RegionName = "Карелия", CapitalName = "Петрозаводск", RegionId = new Guid("248d8071-06e1-425e-a1cf-d1ff4c4a14a8"), CapitalId = new Guid("ccc34487-8fd4-4e71-b032-f4e6c82fb354") },
			new RegionItem { ParentCityCode = 77, Code = 11, RegionName = "Коми", CapitalName = "Сыктывкар", RegionId = new Guid("c20180d9-ad9c-46d1-9eff-d60bc424592a"), CapitalId = new Guid("d2944a73-daf4-4a08-9b34-d9b0af7785a1") },
			new RegionItem { ParentCityCode = 77, Code = 12, RegionName = "Марий Эл", CapitalName = "Йошкар-Ола", RegionId = new Guid("de2cbfdf-9662-44a4-a4a4-8ad237ae4a3e"), CapitalId = new Guid("0648e41c-a09b-4eac-91cd-8cf61b9ccb7b") },
			new RegionItem { ParentCityCode = 77, Code = 13, RegionName = "Мордовия", CapitalName = "Саранск", RegionId = new Guid("37a0c60a-9240-48b5-a87f-0d8c86cdb6e1"), CapitalId = new Guid("1ccfdc3c-be0f-4e42-ab4d-98f90de972d9") },
			new RegionItem { ParentCityCode = null, Code = 14, RegionName = "Саха Якутия", CapitalName = "Якутск", RegionId = new Guid("c225d3db-1db6-4063-ace0-b3fe9ea3805f"), CapitalId = new Guid("884c84a2-0141-4652-962d-8a92989b88f7") },
			new RegionItem { ParentCityCode = null, Code = 15, RegionName = "Северная Осетия - Алания", CapitalName = "Владикавказ", RegionId = new Guid("de459e9c-2933-4923-83d1-9c64cfd7a817"), CapitalId = new Guid("20ea2341-4f49-4c5c-a9dc-a54688c8cc61") },
			new RegionItem { ParentCityCode = 77, Code = 16, RegionName = "Татарстан", CapitalName = "Казань", RegionId = new Guid("0c089b04-099e-4e0e-955a-6bf1ce525f1a"), CapitalId = new Guid("93b3df57-4c89-44df-ac42-96f05e9cd3b9") },
			new RegionItem { ParentCityCode = null, Code = 17, RegionName = "Тыва", CapitalName = "Кызыл", RegionId = new Guid("026bc56f-3731-48e9-8245-655331f596c0"), CapitalId = new Guid("8df8c56f-a46e-438f-a85b-a9b18ad4fc77") },
			new RegionItem { ParentCityCode = 77, Code = 18, RegionName = "Удмуртская", CapitalName = "Ижевск", RegionId = new Guid("52618b9c-bcbb-47e7-8957-95c63f0b17cc"), CapitalId = new Guid("deb1d05a-71ce-40d1-b726-6ba85d70d58f") },
			new RegionItem { ParentCityCode = null, Code = 19, RegionName = "Хакасия", CapitalName = "Абакан", RegionId = new Guid("8d3f1d35-f0f4-41b5-b5b7-e7cadf3e7bd7"), CapitalId = new Guid("42a02e11-a337-4d50-8596-fc76dae7c62a") },
			new RegionItem { ParentCityCode = null, Code = 20, RegionName = "Чеченская", CapitalName = "Грозный", RegionId = new Guid("de67dc49-b9ba-48a3-a4cc-c2ebfeca6c5e"), CapitalId = new Guid("a2072dc5-45be-4db3-ab13-10784ba8b2ae") },
			new RegionItem { ParentCityCode = 77, Code = 21, RegionName = "Чувашская Республика -", CapitalName = "Чебоксары", RegionId = new Guid("878fc621-3708-46c7-a97f-5a13a4176b3e"), CapitalId = new Guid("dd8caeab-c685-4f2a-bf5f-550aca1bbc48") },
			new RegionItem { ParentCityCode = null, Code = 22, RegionName = "Алтайский", CapitalName = "Барнаул", RegionId = new Guid("8276c6a1-1a86-4f0d-8920-aba34d4cc34a"), CapitalId = new Guid("d13945a8-7017-46ab-b1e6-ede1e89317ad") },
			new RegionItem { ParentCityCode = null, Code = 23, RegionName = "Краснодарский", CapitalName = "Краснодар", RegionId = new Guid("d00e1013-16bd-4c09-b3d5-3cb09fc54bd8"), CapitalId = new Guid("7dfa745e-aa19-4688-b121-b655c11e482f") },
			new RegionItem { ParentCityCode = null, Code = 24, RegionName = "Красноярский", CapitalName = "Красноярск", RegionId = new Guid("db9c4f8b-b706-40e2-b2b4-d31b98dcd3d1"), CapitalId = new Guid("9b968c73-f4d4-4012-8da8-3dacd4d4c1bd") },
			new RegionItem { ParentCityCode = null, Code = 25, RegionName = "Приморский", CapitalName = "Владивосток", RegionId = new Guid("43909681-d6e1-432d-b61f-ddac393cb5da"), CapitalId = new Guid("7b6de6a5-86d0-4735-b11a-499081111af8") },
			new RegionItem { ParentCityCode = 23, Code = 26, RegionName = "Ставропольский", CapitalName = "Ставрополь", RegionId = new Guid("327a060b-878c-4fb4-8dc4-d5595871a3d8"), CapitalId = new Guid("2a1c7bdb-05ea-492f-9e1c-b3999f79dcbc") },
			new RegionItem { ParentCityCode = null, Code = 27, RegionName = "Хабаровский", CapitalName = "Хабаровск", RegionId = new Guid("7d468b39-1afa-41ec-8c4f-97a8603cb3d4"), CapitalId = new Guid("a4859da8-9977-4b62-8436-4e1b98c5d13f") },
			new RegionItem { ParentCityCode = null, Code = 28, RegionName = "Амурская", CapitalName = "Благовещенск", RegionId = new Guid("844a80d6-5e31-4017-b422-4d9c01e9942c"), CapitalId = new Guid("8f41253d-6e3b-48a9-842a-25ba894bd093") },
			new RegionItem { ParentCityCode = 77, Code = 29, RegionName = "Архангельская", CapitalName = "Архангельск", RegionId = new Guid("294277aa-e25d-428c-95ad-46719c4ddb44"), CapitalId = new Guid("06814fb6-0dc3-4bec-ba20-11f894a0faf5") },
			new RegionItem { ParentCityCode = 61, Code = 30, RegionName = "Астраханская", CapitalName = "Астрахань", RegionId = new Guid("83009239-25cb-4561-af8e-7ee111b1cb73"), CapitalId = new Guid("a101dd8b-3aee-4bda-9c61-9df106f145ff") },
			new RegionItem { ParentCityCode = 77, Code = 31, RegionName = "Белгородская", CapitalName = "Белгород", RegionId = new Guid("639efe9d-3fc8-4438-8e70-ec4f2321f2a7"), CapitalId = new Guid("02e9c019-ab4d-4fa0-928e-d6c0a41dc256") },
			new RegionItem { ParentCityCode = 77, Code = 32, RegionName = "Брянская", CapitalName = "Брянск", RegionId = new Guid("f5807226-8be0-4ea8-91fc-39d053aec1e2"), CapitalId = new Guid("414b71cf-921e-4bfc-b6e0-f7395d16aaef") },
			new RegionItem { ParentCityCode = 77, Code = 33, RegionName = "Владимирская", CapitalName = "Владимир", RegionId = new Guid("b8837188-39ee-4ff9-bc91-fcc9ed451bb3"), CapitalId = new Guid("f66a00e6-179e-4de9-8ecb-78b0277c9f10") },
			new RegionItem { ParentCityCode = 61, Code = 34, RegionName = "Волгоградская", CapitalName = "Волгоград", RegionId = new Guid("da051ec8-da2e-4a66-b542-473b8d221ab4"), CapitalId = new Guid("a52b7389-0cfe-46fb-ae15-298652a64cf8") },
			new RegionItem { ParentCityCode = 77, Code = 35, RegionName = "Вологодская", CapitalName = "Вологда", RegionId = new Guid("ed36085a-b2f5-454f-b9a9-1c9a678ee618"), CapitalId = new Guid("023484a5-f98d-4849-82e1-b7e0444b54ef") },
			new RegionItem { ParentCityCode = 77, Code = 36, RegionName = "Воронежская", CapitalName = "Воронеж", RegionId = new Guid("b756fe6b-bbd3-44d5-9302-5bfcc740f46e"), CapitalId = new Guid("5bf5ddff-6353-4a3d-80c4-6fb27f00c6c1") },
			new RegionItem { ParentCityCode = 77, Code = 37, RegionName = "Ивановская", CapitalName = "Иваново", RegionId = new Guid("0824434f-4098-4467-af72-d4f702fed335"), CapitalId = new Guid("40c6863e-2a5f-4033-a377-3416533948bd") },
			new RegionItem { ParentCityCode = null, Code = 38, RegionName = "Иркутская", CapitalName = "Иркутск", RegionId = new Guid("6466c988-7ce3-45e5-8b97-90ae16cb1249"), CapitalId = new Guid("8eeed222-72e7-47c3-ab3a-9a553c31cf72") },
			new RegionItem { ParentCityCode = 77, Code = 39, RegionName = "Калининградская", CapitalName = "Калининград", RegionId = new Guid("90c7181e-724f-41b3-b6c6-bd3ec7ae3f30"), CapitalId = new Guid("df679694-d505-4dd3-b514-4ba48c8a97d8") },
			new RegionItem { ParentCityCode = 77, Code = 40, RegionName = "Калужская", CapitalName = "Калуга", RegionId = new Guid("18133adf-90c2-438e-88c4-62c41656de70"), CapitalId = new Guid("b502ae45-897e-4b6f-9776-6ff49740b537") },
			new RegionItem { ParentCityCode = null, Code = 41, RegionName = "Камчатский", CapitalName = "Петропавловск-Камчатский", RegionId = new Guid("d02f30fc-83bf-4c0f-ac2b-5729a866a207"), CapitalId = new Guid("0b3f0723-5fe0-4c23-af44-8082166c6d2e") },
			new RegionItem { ParentCityCode = null, Code = 42, RegionName = "Кемеровская", CapitalName = "Кемерово", RegionId = new Guid("393aeccb-89ef-4a7e-ae42-08d5cebc2e30"), CapitalId = new Guid("94bb19a3-c1fa-410b-8651-ac1bf7c050cd") },
			new RegionItem { ParentCityCode = 77, Code = 43, RegionName = "Кировская", CapitalName = "Киров", RegionId = new Guid("0b940b96-103f-4248-850c-26b6c7296728"), CapitalId = new Guid("452a2ddf-88a1-4e35-8d8d-8635493768d4") },
			new RegionItem { ParentCityCode = 77, Code = 44, RegionName = "Костромская", CapitalName = "Кострома", RegionId = new Guid("15784a67-8cea-425b-834a-6afe0e3ed61c"), CapitalId = new Guid("14c73394-b886-40a9-9e5c-547cfd4d6aad") },
			new RegionItem { ParentCityCode = null, Code = 45, RegionName = "Курганская", CapitalName = "Курган", RegionId = new Guid("4a3d970f-520e-46b9-b16c-50d4ca7535a8"), CapitalId = new Guid("3bbda77d-ba3f-4457-9d44-c440815cda89") },
			new RegionItem { ParentCityCode = 77, Code = 46, RegionName = "Курская", CapitalName = "Курск", RegionId = new Guid("ee594d5e-30a9-40dc-b9f2-0add1be44ba1"), CapitalId = new Guid("d790c72e-479b-4da2-90d7-842b1712a71c") },
			new RegionItem { ParentCityCode = 77, Code = 47, RegionName = "Ленинградская", CapitalName = "Санкт-Петербург", RegionId = new Guid("c2deb16a-0330-4f05-821f-1d09c93331e6"), CapitalId = new Guid("c2deb16a-0330-4f05-821f-1d09c93331e6") },
			new RegionItem { ParentCityCode = 77, Code = 48, RegionName = "Липецкая", CapitalName = "Липецк", RegionId = new Guid("1490490e-49c5-421c-9572-5673ba5d80c8"), CapitalId = new Guid("eacb5f15-1a2e-432e-904a-ca56bd635f1b") },
			new RegionItem { ParentCityCode = null, Code = 49, RegionName = "Магаданская", CapitalName = "Магадан", RegionId = new Guid("9c05e812-8679-4710-b8cb-5e8bd43cdf48"), CapitalId = new Guid("cb8ae35a-93df-4133-b377-50f3698c8b5e") },
			new RegionItem { ParentCityCode = 77, Code = 50, RegionName = "Московская", CapitalName = "Москва", RegionId = new Guid("0c5b2444-70a0-4932-980c-b4dc0d3f02b5"), CapitalId = new Guid("0c5b2444-70a0-4932-980c-b4dc0d3f02b5") },
			new RegionItem { ParentCityCode = null, Code = 51, RegionName = "Мурманская", CapitalName = "Мурманск", RegionId = new Guid("1c727518-c96a-4f34-9ae6-fd510da3be03"), CapitalId = new Guid("b7127184-ead6-422b-b7cf-4fc7725b47a5") },
			new RegionItem { ParentCityCode = 77, Code = 52, RegionName = "Нижегородская", CapitalName = "Нижний Новгород", RegionId = new Guid("88cd27e2-6a8a-4421-9718-719a28a0a088"), CapitalId = new Guid("555e7d61-d9a7-4ba6-9770-6caa8198c483") },
			new RegionItem { ParentCityCode = 77, Code = 53, RegionName = "Новгородская", CapitalName = "Великий Новгород", RegionId = new Guid("e5a84b81-8ea1-49e3-b3c4-0528651be129"), CapitalId = new Guid("8d0a05bf-3b8a-43e9-ac26-7ce61d7c4560") },
			new RegionItem { ParentCityCode = null, Code = 54, RegionName = "Новосибирская", CapitalName = "Новосибирск", RegionId = new Guid("1ac46b49-3209-4814-b7bf-a509ea1aecd9"), CapitalId = new Guid("8dea00e3-9aab-4d8e-887c-ef2aaa546456") },
			new RegionItem { ParentCityCode = null, Code = 55, RegionName = "Омская", CapitalName = "Омск", RegionId = new Guid("05426864-466d-41a3-82c4-11e61cdc98ce"), CapitalId = new Guid("140e31da-27bf-4519-9ea0-6185d681d44e") },
			new RegionItem { ParentCityCode = 77, Code = 56, RegionName = "Оренбургская", CapitalName = "Оренбург", RegionId = new Guid("8bcec9d6-05bc-4e53-b45c-ba0c6f3a5c44"), CapitalId = new Guid("dce97bff-deb2-4fd9-9aec-4f4327bbf89b") },
			new RegionItem { ParentCityCode = 77, Code = 57, RegionName = "Орловская", CapitalName = "Орёл", RegionId = new Guid("5e465691-de23-4c4e-9f46-f35a125b5970"), CapitalId = new Guid("2abed4d9-5565-4885-bc96-f4ffccc6cba4") },
			new RegionItem { ParentCityCode = 77, Code = 58, RegionName = "Пензенская", CapitalName = "Пенза", RegionId = new Guid("c99e7924-0428-4107-a302-4fd7c0cca3ff"), CapitalId = new Guid("ff3292b1-a1d2-47d4-b35b-ac06b50555cc") },
			new RegionItem { ParentCityCode = 77, Code = 59, RegionName = "Пермский", CapitalName = "Пермь", RegionId = new Guid("4f8b1a21-e4bb-422f-9087-d3cbf4bebc14"), CapitalId = new Guid("a309e4ce-2f36-4106-b1ca-53e0f48a6d95") },
			new RegionItem { ParentCityCode = 77, Code = 60, RegionName = "Псковская", CapitalName = "Псков", RegionId = new Guid("f6e148a1-c9d0-4141-a608-93e3bd95e6c4"), CapitalId = new Guid("2858811e-448a-482e-9863-e03bf06bb5d4") },
			new RegionItem { ParentCityCode = null, Code = 61, RegionName = "Ростовская", CapitalName = "Ростов-на-Дону", RegionId = new Guid("f10763dc-63e3-48db-83e1-9c566fe3092b"), CapitalId = new Guid("c1cfe4b9-f7c2-423c-abfa-6ed1c05a15c5") },
			new RegionItem { ParentCityCode = 77, Code = 62, RegionName = "Рязанская", CapitalName = "Рязань", RegionId = new Guid("963073ee-4dfc-48bd-9a70-d2dfc6bd1f31"), CapitalId = new Guid("86e5bae4-ef58-4031-b34f-5e9ff914cd55") },
			new RegionItem { ParentCityCode = 77, Code = 63, RegionName = "Самарская", CapitalName = "Самара", RegionId = new Guid("df3d7359-afa9-4aaa-8ff9-197e73906b1c"), CapitalId = new Guid("bb035cc3-1dc2-4627-9d25-a1bf2d4b936b") },
			new RegionItem { ParentCityCode = 77, Code = 64, RegionName = "Саратовская", CapitalName = "Саратов", RegionId = new Guid("df594e0e-a935-4664-9d26-0bae13f904fe"), CapitalId = new Guid("bf465fda-7834-47d5-986b-ccdb584a85a6") },
			new RegionItem { ParentCityCode = null, Code = 65, RegionName = "Сахалинская", CapitalName = "Южно-Сахалинск", RegionId = new Guid("aea6280f-4648-460f-b8be-c2bc18923191"), CapitalId = new Guid("44388ad0-06aa-49b0-bbf9-1704629d1d68") },
			new RegionItem { ParentCityCode = null, Code = 66, RegionName = "Свердловская", CapitalName = "Екатеринбург", RegionId = new Guid("92b30014-4d52-4e2e-892d-928142b924bf"), CapitalId = new Guid("2763c110-cb8b-416a-9dac-ad28a55b4402") },
			new RegionItem { ParentCityCode = 77, Code = 67, RegionName = "Смоленская", CapitalName = "Смоленск", RegionId = new Guid("e8502180-6d08-431b-83ea-c7038f0df905"), CapitalId = new Guid("d414a2e8-9e1e-48c1-94a4-7308d5608177") },
			new RegionItem { ParentCityCode = 77, Code = 68, RegionName = "Тамбовская", CapitalName = "Тамбов", RegionId = new Guid("a9a71961-9363-44ba-91b5-ddf0463aebc2"), CapitalId = new Guid("ea2a1270-1e19-4224-b1a0-4228b9de3c7a") },
			new RegionItem { ParentCityCode = 77, Code = 69, RegionName = "Тверская", CapitalName = "Тверь", RegionId = new Guid("61723327-1c20-42fe-8dfa-402638d9b396"), CapitalId = new Guid("c52ea942-555e-45c6-9751-58897717b02f") },
			new RegionItem { ParentCityCode = null, Code = 70, RegionName = "Томская", CapitalName = "Томск", RegionId = new Guid("889b1f3a-98aa-40fc-9d3d-0f41192758ab"), CapitalId = new Guid("e3b0eae8-a4ce-4779-ae04-5c0797de66be") },
			new RegionItem { ParentCityCode = 77, Code = 71, RegionName = "Тульская", CapitalName = "Тула", RegionId = new Guid("d028ec4f-f6da-4843-ada6-b68b3e0efa3d"), CapitalId = new Guid("b2601b18-6da2-4789-9fbe-800dde06a2bb") },
			new RegionItem { ParentCityCode = null, Code = 72, RegionName = "Тюменская", CapitalName = "Тюмень", RegionId = new Guid("54049357-326d-4b8f-b224-3c6dc25d6dd3"), CapitalId = new Guid("9ae64229-9f7b-4149-b27a-d1f6ec74b5ce") },
			new RegionItem { ParentCityCode = 77, Code = 73, RegionName = "Ульяновская", CapitalName = "Ульяновск", RegionId = new Guid("fee76045-fe22-43a4-ad58-ad99e903bd58"), CapitalId = new Guid("bebfd75d-a0da-4bf9-8307-2e2c85eac463") },
			new RegionItem { ParentCityCode = null, Code = 74, RegionName = "Челябинская", CapitalName = "Челябинск", RegionId = new Guid("27eb7c10-a234-44da-a59c-8b1f864966de"), CapitalId = new Guid("a376e68d-724a-4472-be7c-891bdb09ae32") },
			new RegionItem { ParentCityCode = null, Code = 75, RegionName = "Забайкальский", CapitalName = "Чита", RegionId = new Guid("b6ba5716-eb48-401b-8443-b197c9578734"), CapitalId = new Guid("2d9abaa6-85a6-4f1f-a1bd-14b76ec17d9c") },
			new RegionItem { ParentCityCode = 77, Code = 76, RegionName = "Ярославская", CapitalName = "Ярославль", RegionId = new Guid("a84b2ef4-db03-474b-b552-6229e801ae9b"), CapitalId = new Guid("6b1bab7d-ee45-4168-a2a6-4ce2880d90d3") },
			new RegionItem { ParentCityCode = null, Code = 77, RegionName = "Москва", CapitalName = "Москва", RegionId = new Guid("0c5b2444-70a0-4932-980c-b4dc0d3f02b5"), CapitalId = new Guid("0c5b2444-70a0-4932-980c-b4dc0d3f02b5") },
			new RegionItem { ParentCityCode = 77, Code = 78, RegionName = "Санкт-Петербург", CapitalName = "Санкт-Петербург", RegionId = new Guid("c2deb16a-0330-4f05-821f-1d09c93331e6"), CapitalId = new Guid("c2deb16a-0330-4f05-821f-1d09c93331e6") },
			new RegionItem { ParentCityCode = null, Code = 79, RegionName = "Еврейская", CapitalName = "Биробиджан", RegionId = new Guid("1b507b09-48c9-434f-bf6f-65066211c73e"), CapitalId = new Guid("5d133391-46ee-496b-83a6-efeeaa903643") },
			new RegionItem { ParentCityCode = null, Code = 83, RegionName = "Ненецкий", CapitalName = "Нарьян-Мар", RegionId = new Guid("89db3198-6803-4106-9463-cbf781eff0b8"), CapitalId = new Guid("b0122c31-eb1c-40ae-b998-08f9e99a0fa1") },
			new RegionItem { ParentCityCode = null, Code = 86, RegionName = "Ханты-Мансийский Автономный округ - Югра", CapitalName = "Ханты-Мансийск", RegionId = new Guid("d66e5325-3a25-4d29-ba86-4ca351d9704b"), CapitalId = new Guid("d680d1a9-ff89-42c0-b39f-143d2ffb520a") },
			new RegionItem { ParentCityCode = null, Code = 87, RegionName = "Чукотский", CapitalName = "Анадырь", RegionId = new Guid("f136159b-404a-4f1f-8d8d-d169e1374d5c"), CapitalId = new Guid("7fad3a21-06b4-4af3-9657-bf1521714952") },
			new RegionItem { ParentCityCode = null, Code = 89, RegionName = "Ямало-Ненецкий", CapitalName = "Салехард", RegionId = new Guid("826fa834-3ee8-404f-bdbc-13a5221cfb6e"), CapitalId = new Guid("0c1b7f05-9fd9-4ec9-a2cc-5ee2799be1e6") },
			new RegionItem { ParentCityCode = 77, Code = 91, RegionName = "Крым", CapitalName = "Симферополь", RegionId = new Guid("bd8e6511-e4b9-4841-90de-6bbc231a789e"), CapitalId = new Guid("b9001e55-72ed-43bf-b7eb-41b86a14380e") },
			new RegionItem { ParentCityCode = 23, Code = 92, RegionName = "Севастополь", CapitalName = "Севастополь", RegionId = new Guid("6fdecb78-893a-4e3f-a5ba-aa062459463b"), CapitalId = new Guid("6fdecb78-893a-4e3f-a5ba-aa062459463b") },
			new RegionItem { ParentCityCode = null, Code = 99, RegionName = "Байконур", CapitalName = "Байконур", RegionId = new Guid("63ed1a35-4be6-4564-a1ec-0c51f7383314"), CapitalId = new Guid("63ed1a35-4be6-4564-a1ec-0c51f7383314") }
		};
	}
}
