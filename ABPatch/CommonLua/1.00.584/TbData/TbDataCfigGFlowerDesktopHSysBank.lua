TbDataCfigGFlowerDesktopHSysBank = TbDataBase:new()

function TbDataCfigGFlowerDesktopHSysBank:new(id)
	o = {}
	setmetatable(o,self)
	self.__index = self
	self.Id = id
	o.Icon = nil
	o.NickName = nil
	o.Golds = nil
	o.Coefficient = 10000

	return o
end

function TbDataCfigGFlowerDesktopHSysBank:load(list_datainfo)
	for i = 0, list_datainfo.Count - 1 do
		local data = list_datainfo[i]
		if(data.data_name == "T_Icon")
		then
			self.Icon = tostring(data.data_value)
		end
		if(data.data_name == "T_NickName")
		then
			self.NickName = tostring(data.data_value)
		end		
		if(data.data_name == "I_Golds")
		then
			self.Golds = tonumber(data.data_value) * self.Coefficient
		end		
	end	
end


TbDataFactoryCfigGFlowerDesktopHSysBank = TbDataFactoryBase:new()

function TbDataFactoryCfigGFlowerDesktopHSysBank:new(o)
	o = o or {}  
    setmetatable(o,self)  
    self.__index = self  		
	
    return o
end

function TbDataFactoryCfigGFlowerDesktopHSysBank:createTbData(id)
	return TbDataCfigGFlowerDesktopHSysBank:new(id)
end